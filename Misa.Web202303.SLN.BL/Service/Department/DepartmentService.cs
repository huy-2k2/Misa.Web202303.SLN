using AutoMapper;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.Common.Emum;
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
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository, mapper)
        {
        }

        /// <summary>
        /// validate khi thêm mới dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected override async Task CreateValidateAsync(DepartmentCreateDto entityCreateDto)
        {
            // kiểm tra mã bộ phận sử dụng trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityCreateDto.Department_code, null);
            if (isCodeExisted)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã bộ phận sử dụng"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã bộ phận sử dụng"),
                    ErrorCode = ErrorCode.DuplicateCode
                };
            }
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task UpdateValidateAsync(Guid id, DepartmentUpdateDto entityUpdateDto)
        {
            // kiểm tra mã bộ phận sử dụng trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityUpdateDto.Department_code, id);
            if (isCodeExisted)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã bộ phận sử dụng"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã bộ phận sử dụng"),
                    ErrorCode = ErrorCode.DuplicateCode
                };
            }
        }
    }
}
