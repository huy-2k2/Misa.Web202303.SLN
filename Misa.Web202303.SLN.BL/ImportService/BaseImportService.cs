using DocumentFormat.OpenXml.Spreadsheet;
using Misa.Web202303.SLN.BL.Service.Dto;
using Misa.Web202303.SLN.BL.ValidateDto;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ImportService
{
    public abstract class BaseImportService<TEntityImportDto, TEntity> : IBaseImportService<TEntity>
    {

        /// <summary>
        /// dùng để gọi phương thức của BaseRepository
        /// </summary>
        private readonly IBaseRepository<TEntity> _baseRepository;

        public BaseImportService(IBaseRepository<TEntity> baseRepository)
        {
            this._baseRepository = baseRepository;
        }

        /// <summary>
        /// validate file excel có đúng định dạng không
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        private async Task FileValdiateAsync(MemoryStream stream)
        {
            // lấy dữ liệu import về table, column
            var listImportEntity = await _baseRepository.GetImportDataAsync();

            // số dòng trong định dạng
            var numberColumn = listImportEntity.First().Number_column;

            try
            {
                using (var package = new ExcelPackage(stream));
            }
            catch (Exception ex)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.FileInvalid,
                    UserMessage = ErrorMessage.FileFormatError
                };
            }

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook?.Worksheets?[0];
                // kiểm tra worksheet có tồn tại
                if (worksheet == null)
                {
                    throw new ValidateException()
                    {
                        ErrorCode = ErrorCode.FileInvalid,
                        UserMessage = ErrorMessage.FileError,
                    };
                }
                // kiểm tra ít nhất có 2 dòng, 1 dòng tiều đề và 1 dòng dữ liệu
                var rowCount = worksheet.Dimension.Rows;
                if (rowCount < 2)
                {
                    throw new ValidateException()
                    {
                        ErrorCode = ErrorCode.FileInvalid,
                        UserMessage = ErrorMessage.FileRowError,
                    };
                }
                // kiểm tra số cột đúng với định dạng trong db
                var columnCount = worksheet.Dimension.Columns;
                if (columnCount != numberColumn)
                {
                    throw new ValidateException()
                    {
                        ErrorCode = ErrorCode.FileInvalid,
                        UserMessage = string.Format(ErrorMessage.FileColumnError, numberColumn)
                    };
                }
            }
        }

        /// <summary>
        /// validate mã code bị trùng
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="errorOfTable"></param>
        /// <returns></returns>
        private async Task<List<List<ValidateError>>> ValidateDuplicateCodeAsync(IEnumerable<TEntityImportDto> listEntity, IEnumerable<IEnumerable<ValidateError>> errorOfTable)
        {
            var tableName = _baseRepository.GetTableName();
            var listCode = new List<string>();
            var result = new List<List<ValidateError>>();
            for (int i = 0; i < listEntity.Count(); i++)
            {
                var entity = listEntity.ElementAt(i);
                var prop = entity.GetType().GetProperty($"{tableName}_code");
                var codeValue = (string)prop.GetValue(entity);
                // lấy ra mã code vào add vào listCode
                listCode.Add(codeValue);
            }
            // gọi repository để kiểm tra, truyền lên danh sách mã code cách nhau bởi dấu ,
            var listCodeExisted = await _baseRepository.GetListExistedCodeAsync(string.Join(",", listCode));

            for (int i = 0; i < listCode.Count; i++)
            {
                var codeValue = listCode[i];
                var errorOfRow = errorOfTable.ElementAt(i).ToList();
                // kiểm tra mã code có tồn tại trong dánh sách code lấy từ db, nếu có thì add thêm lỗi vào dòng dữ liệu đó
                if (listCodeExisted.Contains(codeValue))
                {
                    errorOfRow.Add(new ValidateError()
                    {
                        FieldNameError = $"{tableName}_code",
                        Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                    });
                }

                // kiểm tra nếu mã code có tồn tại trong danh sách code phía trên
                for(int j = 0; j < i; j++)
                {
                    if (listCode[j] == listCode[i])
                    {
                        errorOfRow.Add(new ValidateError()
                        {
                            FieldNameError = $"{tableName}_code",
                            Message = string.Format(ErrorMessage.DuplicateCodeAboveError, j + 1),
                        });
                        break;
                    }
                }

                result.Add(errorOfRow);
            }
            return result;
        }

        /// <summary>
        /// validate tất cả
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<ImportErrorEntity<TEntity>> ValidateAsync(MemoryStream stream)
        {
            // validate file exccel
            await FileValdiateAsync(stream);
            // lấy ra dữ liệu import về table và column
            var listImportData = await _baseRepository.GetImportDataAsync();
            // biến lưu dữ liệu lấy ra từ file excel khi chưa quan tâm đến validate
            var rawEntities = new List<List<string>>();
            var tableName = _baseRepository.GetTableName();
            var listEntityImport = new List<TEntityImportDto>();
            // dữ liệu về tiêu đề của bảng excel
            var tHead = new List<string>();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook?.Worksheets?[0];
                var rowCount = worksheet.Dimension.Rows;
                var columnCount = worksheet.Dimension.Columns;

                // lấy dữ liệu về tiêu đề của table excel
                for (int i = 1; i <= columnCount; i++)
                {
                    var th = worksheet.Cells[1, i].Value.ToString()?.Trim();
                    tHead.Add(th);
                }

                // tạo biến lưu lỗi của cả table
                var errorOfTable = new List<List<ValidateError>>();
                for (int row = 2; row <= rowCount; row++)
                {
                    var entityImport = (TEntityImportDto)Activator.CreateInstance(typeof(TEntityImportDto));
                    var errorOfRow = new List<ValidateError>();
                    var rawEntity = new List<string>();
                    for (int column = 1; column <= columnCount; column++)
                    {
                        var temp = worksheet.Cells[row, column].Value?.ToString()?.Trim();
                        var value = temp ?? "";
                        // thêm dữ liệu string vào rawEntity
                        rawEntity.Add(value);
                        // lấy ra dữ liệu import tương ứng với column trong file excel
                        var importData = listImportData.ToList().Find(imp => imp.Import_column_index == column);
                        // validate kiểu dữ liệu
                        var isDataTypeValid = ValidateDataType.Validate(importData.Data_type, value);
                        if (!isDataTypeValid)
                        {
                            errorOfRow.Add(new ValidateError()
                            {
                                FieldNameError = importData.Prop_name,
                                Message = ErrorMessage.DataTypeError
                            });
                        }
                        else
                        {
                            var prop = entityImport.GetType().GetProperty(importData.Prop_name);
                            prop.SetValue(entityImport, Convert.ChangeType(value, prop.PropertyType), null);
                        }
                    }
                    listEntityImport.Add(entityImport);
                    if (errorOfRow.Count == 0)
                    {
                        //validate attribute
                        errorOfRow = ValidateAttribute.Validate(entityImport).ToList();

                        // validate business
                        errorOfRow = errorOfRow.Concat(ValidateBusiness(entityImport)).ToList();
                    }

                    errorOfTable.Add(errorOfRow);
                    rawEntities.Add(rawEntity);
                }
                // validate trùng code
                errorOfTable = await ValidateDuplicateCodeAsync(listEntityImport, errorOfTable);

                // validate khóa ngoại tồn tại
                errorOfTable = await ValidateForeignKeyAsync(listEntityImport, errorOfTable);

                var isPassed = errorOfTable.Where(errorOfRow => errorOfRow.Count > 0).Count() == 0;

                var listEntity = isPassed ? await MapToListEntity(listEntityImport) : null;

                return new ImportErrorEntity<TEntity>()
                {
                    ErrorOfTable = errorOfTable,
                    ListImportData = listImportData,
                    IsPassed = isPassed,
                    RawEntities = rawEntities,
                    ListEntity = listEntity,
                    THead = tHead
                };
            }
        }

        /// <summary>
        /// validate khóa ngoại
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="errorOfTable"></param>
        /// <returns></returns>
        protected virtual async Task<List<List<ValidateError>>> ValidateForeignKeyAsync(IEnumerable<TEntityImportDto> listEntity, IEnumerable<IEnumerable<ValidateError>> errorOfTable) {
            return errorOfTable.Select(errorOfRow => errorOfRow.ToList()).ToList();
        }

        /// <summary>
        /// validate nghiệp vụ 
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual List<ValidateError> ValidateBusiness(TEntityImportDto entity) {
            return new List<ValidateError>();
        }

        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity"></param>
        /// <returns></returns>
        protected abstract Task<List<TEntity>> MapToListEntity(IEnumerable<TEntityImportDto> listImportEntity);
    }
}
