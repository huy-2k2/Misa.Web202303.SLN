using Misa.Web202303.QLTS.BL.Service.Department;
using Misa.Web202303.QLTS.BL.Service.License;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository.License;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.License
{
    public class LicenseDomainService : ILicenseDomainService
    {

        private readonly ILicenseRepository _licenseRepository;

        public LicenseDomainService(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }

        public async Task CreateValidateAsync(LicenseCreateDto licenseCreateDto)
        {
            var listError = ValidateAttribute.Validate(licenseCreateDto);

            var isCodeExisted = await _licenseRepository.CheckCodeExistedAsync(licenseCreateDto.license_code, null);
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
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))
                };
            }
        }

        public async Task UpdateValidateAsync(Guid licenseId, LicenseUpdateDto licenseUpdateDto)
        {
            var listError = ValidateAttribute.Validate(licenseUpdateDto);

            var isCodeExisted = await _licenseRepository.CheckCodeExistedAsync(licenseUpdateDto.license_code, licenseId);
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
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))
                };
            }

        }
        public async Task DeleteListValidateAsync(IEnumerable<Guid> listId)
        {
            var listError = new List<ValidateError>();
            var stringIds = string.Join(",", listId);
            var listExisted = await _licenseRepository.GetListExistedAsync(stringIds);

            if (listExisted.Count() != listId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.License)
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))
                };
            }
        }

        public void FilterInputValdiate(int pageSize, int currentPage)
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
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateFilterError
                };
            }
        }

    }
}
