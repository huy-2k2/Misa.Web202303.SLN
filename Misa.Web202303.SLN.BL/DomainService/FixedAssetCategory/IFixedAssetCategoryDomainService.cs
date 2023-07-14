using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedAssetCategoryEntity = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;


namespace Misa.Web202303.QLTS.BL.DomainService.FixedAssetCategory
{
    public interface IFixedAssetCategoryDomainService
    {
        Task CreateValidateAsync(FixedAssetCategoryCreateDto fixedAssetCategoryCreateDto);

        Task UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto fixedAssetCategoryUpdateDto);

        List<ValidateError> BusinessValidate(FixedAssetCategoryEntity fixedAssetCategory);

    }
}
