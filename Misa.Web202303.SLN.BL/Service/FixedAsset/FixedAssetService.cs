using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Misa.Web202303.SLN.BL.ImportService;
using Misa.Web202303.SLN.BL.ImportService.FixedAsset;
using Misa.Web202303.SLN.BL.Service.Dto;
using Misa.Web202303.SLN.BL.ValidateDto;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository.Department;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

// vì FixedAsset trong Entity ở DL trùng với tên của namespace nên đặt bí danh
using FixedAssetEntity = Misa.Web202303.SLN.DL.Entity.FixedAsset;

namespace Misa.Web202303.SLN.BL.Service.FixedAsset
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của FixedAsset, gồm các phương thức của IFixedAssetService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary
    public class FixedAssetService : BaseService<FixedAssetEntity, FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>, IFixedAssetService
    {
        /// <summary>
        /// sử dụng để gọi phương thức của IFixedAssetRepository
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// gọi phương thức của IDepartmentRepository
        /// </summary>
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// gọi phương thức của IFixedAssetCategoryRepository
        /// </summary>
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;

        /// <summary>
        /// sử dụng để gọi phương thức import từ file exccel
        /// </summary>
        private readonly IFixedAssetImportService _fixedAssetImportService;

        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IMapper mapper, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IFixedAssetImportService fixedAssetImportService) : base(fixedAssetRepository, mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _fixedAssetImportService = fixedAssetImportService;
        }


        /// <summary>
        /// filter, search, phân trang tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            // validate dữ liệu
            // pageSize lớn hơn 0
            var listError = new List<ValidateError>();
            if (pageSize <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "pageSize",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.PageSize),
                });
            }
            // current page lớn hơn 0
            if (currentPage <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "currentPage",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.CurrentPage),
                });
            }
            // department tồn tại
            if (departmentId != null)
            {
                var department = await _departmentRepository.GetAsync((Guid)departmentId);
                if (department == null)
                {
                    listError.Add(new ValidateError()
                    {
                        FieldNameError = "department_id",
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.DepartmentCode),
                    });
                }
            }
            // loại tài sản tồn tại
            if (fixedAssetCategoryId != null)
            {
                var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync((Guid)fixedAssetCategoryId);
                if (fixedAssetCategory == null)
                {
                    listError.Add(new ValidateError()
                    {
                        FieldNameError = "fixed_asset_category_id",
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                    });
                }
            }
            // nếu có lỗi thì throw excception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateFilterError
                };
            }
            var filterListFixedAsset = await _fixedAssetRepository.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
            var listFixedAsset = filterListFixedAsset.List_fixed_asset.Select((fixedAsset) => _mapper.Map<FixedAssetDto>(fixedAsset));

            return new { listFixedAsset, totalAsset = filterListFixedAsset.Total_asset, totalQuantity = filterListFixedAsset.Total_quantity, totalCost = filterListFixedAsset.Total_cost };
        }


        /// <summary>
        /// tự động sinh mà tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRecommendFixedAssetCodeAsync()
        {
            // lấy ra tiền tố và mã tài sản có hậu tố lớn nhất
            var tempList = await _fixedAssetRepository.GetMaxAssetCodeAsync();
            // lấy ra tiền tố ở ví trị 0
            var prefixCode = tempList.First();
            // lấy ra mã tài sản ở vị trí 1
            var maxAssetCode = tempList.Last();

            // lấy ra hậu tố của mã tài sản
            var postFix = $"{maxAssetCode.Substring(prefixCode.Length)}";

            // tính toán hậu tố của mã tài sản mới
            var newPostFix = "0";
            if (postFix.Length != 0)
                newPostFix = $"{long.Parse(postFix) + 1}";

            // cộng thêm các chữ số 0 vào hậu tố của mã tài sản mới
            for (int i = 0; i < postFix.Length; i++)
            {
                if (postFix[i] != '0')
                    break;
                if (newPostFix.Length < postFix.Length)
                    newPostFix = '0' + newPostFix;
            }

            return prefixCode + newPostFix;
        }


        /// <summary>
        /// validate khi thêm mới dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task<List<ValidateError>> CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
            var listError = new List<ValidateError>();
            // gọi hàm validate chung
            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetCreateDto);

            var businessErrors = BusinessValidate(fixedAsset);

            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAsset);

            listError = Enumerable.Concat(listError, businessErrors).ToList();

            listError = Enumerable.Concat(listError, foreignKeyErrors).ToList();
            // kiểm tra mã tài sản bị trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(fixedAssetCreateDto.Fixed_asset_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Fixed_asset_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAssetCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId"></param>
        /// <param name="fixedAssetUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task<List<ValidateError>> UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            var listError = new List<ValidateError>();
            // gọi hàm validate chung
            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetUpdateDto);

            var businessErrors = BusinessValidate(fixedAsset);

            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAsset);

            listError = Enumerable.Concat(listError, businessErrors).ToList();

            listError = Enumerable.Concat(listError, foreignKeyErrors).ToList();

            // kiểm tra mã tài sản bị trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(fixedAssetUpdateDto.Fixed_asset_code, fixedAssetId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Fixed_asset_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAssetCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// hàm validate chung cho cả insert và update, thực hiện các logic nghiệp vụ
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        private async Task<List<ValidateError>> ForeignKeyValidateAsync(FixedAssetEntity fixedAsset)
        {
            var listError = new List<ValidateError>();
            // kiểm tra department_id tồn tại
            var department = await _departmentRepository.GetAsync(fixedAsset.Department_id);
            if (department == null)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "DepartmentCode",
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.DepartmentCode),
                });
            }
            // kiểm tra fixed_asset_category_id tồn tại
            var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync(fixedAsset.Fixed_asset_category_id);
            if (fixedAssetCategory == null)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Fixed_asset_category_id",
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// validate nghiệp vụ
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <returns></returns>
        public static List<ValidateError> BusinessValidate(FixedAssetEntity fixedAsset)
        {
            var listError = new List<ValidateError>();
            // làm tròn số
            var depreciationAnnual = Math.Round((double)fixedAsset.Depreciation_rate * fixedAsset.Cost / 100, 2);
            var depreciationRate = Math.Round((double)1 / fixedAsset.Life_Time * 100, 2);
            // hao mòn năm  = tỷ lệ hao mòn * nguyên giá
            if (fixedAsset.Depreciation_annual != depreciationAnnual)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Depreciation_annual",
                    Message = ErrorMessage.DepAnnualCostDepRateError,
                });
            }
            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if (fixedAsset.Depreciation_rate != depreciationRate)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Depreciation_rate",
                    Message = ErrorMessage.DepRateLifeTimeError,
                });
            }
            // ngày mua nhỏ hơn hoạc bằng ngày sử dụng
            if(fixedAsset.Use_date < fixedAsset.Purchase_date)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "Use_date",
                    Message = ErrorMessage.UseDateLessThanPurchaseDateError,
                });
            }
            return listError;
        }

        /// <summary>
        /// import dữ liệu tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        public async Task<ImportErrorEntity<FixedAssetEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit)
        {
            // validate dữ liệu
            var validateEntity = await _fixedAssetImportService.ValidateAsync(stream);
            if (isSubmit && validateEntity.IsPassed || !isSubmit)
            {
                var listEntity = validateEntity.ListEntity;
                // nếu không có lỗi thì import
                if (isSubmit)
                    await _baseRepository.InsertListAsync(listEntity);
                return validateEntity;
            }
            else
            {
                // có lỗi thì throw exception
                throw new ValidateException()
                {
                    Data = validateEntity,
                    ErrorCode = ErrorCode.InvalidData,
                    UserMessage = ErrorMessage.FileDataError
                };
            }
        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override string GetAssetName()
        {
            return AssetName.FixedAsset;
        }
    }
}
