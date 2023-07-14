using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.DomainService.FixedAsset
{
    public interface IFixedAssetDomainService
    {
        Task ValidateInputFilterAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId);

        Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto);

        Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto);

        Task GetFilterNoLicenseValidateAsync(FilterFixedAssetNoLicense input);

        List<ValidateError> BusinessValidate(FixedAssetEntity fixedAsset);


    }
}
