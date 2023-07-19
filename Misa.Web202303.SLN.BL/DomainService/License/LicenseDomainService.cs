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
        /// <summary>
        /// dùng để gọi repo của license
        /// </summary>
        private readonly ILicenseRepository _licenseRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="licenseRepository">LicenseRepo</param>
        public LicenseDomainService(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }


        /// <summary>
        /// hàm valdiate khi thêm mới chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="licenseCreateDto">đối tượng LicenseCreateDto</param>
        /// <exception cref="ValidateException">throw excception khi validate thấy lỗi</exception>
        /// <returns></returns>
        public async Task CreateValidateAsync(LicenseCreateDto licenseCreateDto)
        {
            /// validate attribute
            var listError = ValidateAttribute.Validate(licenseCreateDto);

            /// kiểm tra mã trùng
            var isCodeExisted = await _licenseRepository.CheckCodeExistedAsync(licenseCreateDto.license_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // nếu có lỗi thì throw exception
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
        /// hàm validate khi update chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="licenseId">id chứng từ</param>
        /// <param name="licenseUpdateDto">đối tượng LicenseUpdateDto</param>
        /// <exception cref="ValidateException">throw excception khi validate thấy lỗi</exception>
        /// <returns></returns>
        public async Task UpdateValidateAsync(Guid licenseId, LicenseUpdateDto licenseUpdateDto)
        {
            /// validate attribute
            var listError = ValidateAttribute.Validate(licenseUpdateDto);

            /// validate mã trùng
            var isCodeExisted = await _licenseRepository.CheckCodeExistedAsync(licenseUpdateDto.license_code, licenseId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateUpdateError
                };
            }

        }

        /// <summary>
        /// hàm validate khi xóa nhiều chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="listId">danh sách id chứng từ cần xóa</param>
        /// <exception cref="ValidateException">throw excception khi validate thấy lỗi</exception>
        /// <returns></returns>
        public async Task DeleteListValidateAsync(IEnumerable<Guid> listId)
        {
            var listError = new List<ValidateError>();
            var stringIds = string.Join(",", listId);
            // kiểm tra xem trong danh sách id có id nào không tồn tại hay không
            var listExisted = await _licenseRepository.GetListExistedAsync(stringIds);

            if (listExisted.Count() != listId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.License)
                });
            }

            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.DataError
                };
            }
        }

        /// <summary>
        /// phương thức validate khi filter dữ liệu (kiểm tra xem dữ liệu để filter có hợp lệ không)
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="pageSize">kích thước page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <exception cref="ValidateException">throw excception khi validate thấy lỗi</exception>
        /// <returns></returns>
        public void FilterInputValdiate(int pageSize, int currentPage)
        {
            // validate dữ liệu
            // pageSize lớn hơn 0
            var listError = new List<ValidateError>();
            if (pageSize <= 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.PageSize),
                });
            }
            // current page lớn hơn 0
            if (currentPage <= 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.CurrentPage),
                });
            }
            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateFilterError
                };
            }
        }

    }
}
