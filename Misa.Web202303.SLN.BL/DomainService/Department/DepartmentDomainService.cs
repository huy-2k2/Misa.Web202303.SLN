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

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.BL.DomainService.Department
{
    public class DepartmentDomainService : IDepartmentDomainService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentDomainService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task CreateValidateAsync(DepartmentCreateDto departmentCreateDto)
        {
            var listError = new List<ValidateError>();
            var isCodeExisted = await _departmentRepository.CheckCodeExistedAsync(departmentCreateDto.department_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))

                };
            }
        }

        public async Task UpdateValidateAsync(Guid deparmtentId, DepartmentUpdateDto departmentUpdateDto)
        {
            var listError = new List<ValidateError>();
            var isCodeExisted = await _departmentRepository.CheckCodeExistedAsync(departmentUpdateDto.department_code, deparmtentId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            var isExisted = await _departmentRepository.GetAsync(deparmtentId) != null;
            if (!isExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.Department),
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))

                };
            }
        }
    }

}
