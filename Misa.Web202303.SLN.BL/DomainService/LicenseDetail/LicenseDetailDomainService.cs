using Misa.Web202303.QLTS.BL.Service.LicenseDetail;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.DL.Repository.LicenseDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.LicenseDetail
{
    public class LicenseDetailDomainService : ILicenseDetailDomainService
    {
        /// <summary>
        /// dùng để gọi repo fixed asset
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// dùng để gọi service license
        /// </summary>
        private readonly ILicenseRepository _licenseRepository;
        /// <summary>
        /// dùng để gọi service của license detail
        /// </summary>
        private readonly ILicenseDetailRepository _licenseDetailRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="fixedAssetRepository">fixedAssetRepository</param>
        /// <param name="licenseRepository">licenseRepository</param>
        /// <param name="licenseDetailRepository">licenseDetailRepository</param>
        public LicenseDetailDomainService(IFixedAssetRepository fixedAssetRepository, ILicenseRepository licenseRepository, ILicenseDetailRepository licenseDetailRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _licenseRepository = licenseRepository;
            _licenseDetailRepository = licenseDetailRepository;
        }


        /// <summary>
        /// hàm validate create khi thêm license detail
        /// </summary>
        /// <param name="listCreateDto">listCreateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception nếu validate thấy lỗi</exception>
        public async Task CreateListValidateAsync(IEnumerable<LicenseDetailCreateDto> listCreateDto)
        {
            var listError = new List<ValidateError>();
            
            // lấy danh sách id tài sản
            var listFAId = listCreateDto.Select(dto => dto.fixed_asset_id);
            var stringFAId = string.Join(",", listFAId);

            /// danh sách tài sản thực sự tồn tại trong list id tài sản
            var listFAExisted = await _fixedAssetRepository.GetListExistedAsync(stringFAId);

            // trường hợp list create không có bản ghi nào thì thêm lỗi (vì chứng từ phải có ít nhất 1 tài sản)
            if (listCreateDto.Count() == 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.SelectFixedAssetMinError
                });
            }

            // nếu trong list tài sản có ít nhất 1 tài sản không tồn tại thì báo lỗi
            if (listFAExisted.Count() != listFAId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset)
                });
            }

            // kiểm tra license id có tồn tại hay không
            var listLicenseId = listCreateDto.Select(dto => dto.license_id);
            var stringLicenseId = string.Join(",", listLicenseId);
            var listLicenseExisted = await _licenseRepository.GetListExistedAsync(stringLicenseId);
            // nếu license id không tồn tại thì thêm lỗi
            if (listLicenseExisted.Count() != listLicenseId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.License)
                });
            }

            // kiểm tra xem tài sản đã tồn tại trong chứng từ nào chưa (vì 1 tài sản chỉ thuộc về 1 chứng từ)
            var listdExistedFAId = await _licenseDetailRepository.GetListFAExistedAsync(stringFAId);
            if (listdExistedFAId.Count() > 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAsset)
                });
            }

            // nếu list lỗi có length > 0 thì báo lỗi
            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.ValidateCreateError,
                    Data = listError
                };
            }
        }

        /// <summary>
        /// hàm validate khi xóa nhiều license detail của 1 chứng từ
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="licenseId">id của license tương ứng</param>
        /// <param name="listDetailId">danh sách list license detail id</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception khi có lỗi</exception>
        public async Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId)
        {
            var listError = new List<ValidateError>();
            // kiểm tra xem list license detail id có thực sự tồn tại, và có thuộc vào chứng từ có id đã cho hay không
            var listExisted = await _licenseDetailRepository.GetListExistedOfLicenseAsync(licenseId, string.Join(",", listDetailId));
            if (listExisted.Count() != listDetailId.Count())
            {
                listError.Add(
                    new ValidateError()
                    {
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.License)
                    }
                );
            }

            // có lỗi thì throw exception
            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DataError,
                    Data = listError
                };
            }
        }
    }
}
