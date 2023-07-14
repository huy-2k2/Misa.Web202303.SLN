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
        private readonly IFixedAssetRepository _fixedAssetRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILicenseDetailRepository _licenseDetailRepository;
        public LicenseDetailDomainService(IFixedAssetRepository fixedAssetRepository, ILicenseRepository licenseRepository, ILicenseDetailRepository licenseDetailRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _licenseRepository = licenseRepository;
            _licenseDetailRepository = licenseDetailRepository;
        }

        public async Task CreateListValidateAsync(IEnumerable<LicenseDetailCreateDto> listCreateDto)
        {
            var listError = new List<ValidateError>();
            var listFAId = listCreateDto.Select(dto => dto.fixed_asset_id);
            var stringFAId = string.Join(",", listFAId);

            var listFAExisted = await _fixedAssetRepository.GetListExistedAsync(stringFAId);
            
            if(listCreateDto.Count() == 0) {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.SelectFixedAssetMinError
                });
            }

            if(listFAExisted.Count() != listFAId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset)
                });
            }

            var listLicenseId = listCreateDto.Select(dto => dto.license_id);
            var stringLicenseId = string.Join(",", listLicenseId);
            var listLicenseExisted = await _licenseRepository.GetListExistedAsync(stringLicenseId);
            if(listLicenseExisted.Count() != listLicenseId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.License)
                });
            }

            var listdExistedFAId = await _licenseDetailRepository.GetListFAExistedAsync(stringFAId);
            if(listdExistedFAId.Count() > 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAsset)
                });
            }

            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))
                };
            }
        }

        public async Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId)
        {
            var listExisted = await _licenseDetailRepository.GetListExistedOfLicenseAsync(licenseId, string.Join(",", listDetailId));
            if(listExisted.Count() != listDetailId.Count())
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.InvalidError, FieldName.License)
                };
            }
        }
    }
}
