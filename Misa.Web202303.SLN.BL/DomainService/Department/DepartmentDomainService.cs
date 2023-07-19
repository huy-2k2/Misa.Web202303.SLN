using Misa.Web202303.QLTS.BL.Service.Department;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Misa.Web202303.QLTS.BL.DomainService.Department
{
    public class DepartmentDomainService : IDepartmentDomainService
    {
        /// <summary>
        /// repo để gọi department
        /// </summary>
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (05/06/2023)
        /// </summary>
        /// <param name="departmentRepository">departmentRepository</param>
        public DepartmentDomainService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }



        /// <summary>
        /// validate khi thêm mới 
        /// created by: NQ Huy (05/06/2023)
        /// </summary>
        /// <param name="departmentCreateDto">đối tượng DepartmentCreateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception khi gặp lỗi</exception>
        public async Task CreateValidateAsync(DepartmentCreateDto departmentCreateDto)
        {
            var listError = new List<ValidateError>();
            // kiểm tra mã trùng
            var isCodeExisted = await _departmentRepository.CheckCodeExistedAsync(departmentCreateDto.department_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // throw exception nếu có lỗi
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateCreateError

                };
            }
        }

        /// <summary>
        /// validate khi update
        /// created by: NQ Huy (05/06/2023)
        /// </summary>
        /// <param name="deparmtentId">id của đối tượng update</param>
        /// <param name="departmentUpdateDto">đối tượng DepartmentUpdateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception khi gặp lỗi</exception>
        public async Task UpdateValidateAsync(Guid deparmtentId, DepartmentUpdateDto departmentUpdateDto)
        {
            var listError = new List<ValidateError>();
            
            // kiểm tra mã trùng
            var isCodeExisted = await _departmentRepository.CheckCodeExistedAsync(departmentUpdateDto.department_code, deparmtentId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // kiểm tra xem departemnt id có tồn tại không
            var isExisted = await _departmentRepository.GetAsync(deparmtentId) != null;
            if (!isExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.Department),
                });
            }

            // throw exception nếu có lỗi
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateUpdateError

                };
            }
        }
    }

}
