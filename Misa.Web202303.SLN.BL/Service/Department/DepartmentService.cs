using AutoMapper;
using Misa.Web202303.SLN.BL.ImportService;
using Misa.Web202303.SLN.BL.ImportService.Department;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.BL.ValidateDto;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của Department, gồm các phương thức của IDepartmentService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentService : BaseService<DepartmentEntity, DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>, IDepartmentService
    {
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentRepository"></param>
        /// <param name="mapper"></param>

        private readonly IDepartmentImportService _departmentImportService;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, IDepartmentImportService departmentImportService) : base(departmentRepository, mapper)
        {
            _departmentImportService = departmentImportService;
        }

        /// <summary>
        /// import dữ liệu phòng ban từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        public async Task<ImportErrorEntity<DepartmentEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit)
        {
            // validate dữ liệu
            var validateEntity = await _departmentImportService.ValidateAsync(stream);
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
        /// validate khi thêm mới dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected override async Task<List<ValidateError>> CreateValidateAsync(DepartmentCreateDto entityCreateDto)
        {
            var listError = new List<ValidateError>();
            // kiểm tra mã bộ phận sử dụng trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityCreateDto.Department_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "department_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.DepartmentCode)
                });
            }
            return listError;
        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override string GetAssetName()
        {
            return AssetName.Department;
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task<List<ValidateError>> UpdateValidateAsync(Guid id, DepartmentUpdateDto entityUpdateDto)
        {
            var listError = new List<ValidateError>();
            // kiểm tra mã bộ phận sử dụng trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityUpdateDto.Department_code, id);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "department_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.DepartmentCode),
                });
            }
            return listError;   
        }
    }
}
