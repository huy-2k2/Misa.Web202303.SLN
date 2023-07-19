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
        /// <summary>
        /// hàm validate khi create 
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryCreateDto">đối tượng FixedAssetCategoryCreateDto</param>
        /// <returns></returns>
        Task CreateValidateAsync(FixedAssetCategoryCreateDto fixedAssetCategoryCreateDto);

        /// <summary>
        /// hàm validate khi update fixed asset
        /// </summary>
        /// created by: NQ Huy (08/07/2023)
        /// <param name="id">id của đối tượng update</param>
        /// <param name="fixedAssetCategoryUpdateDto">đối tượng FixedAssetCategoryUpdateDto</param>
        /// <returns></returns>
        Task UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto fixedAssetCategoryUpdateDto);

        /// <summary>
        /// hàm validate business
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="fixedAssetCategory">entity FixedAssetCategory</param>
        /// <returns>danh sách lỗi</returns>
        List<ValidateError> BusinessValidate(FixedAssetCategoryEntity fixedAssetCategory);

    }
}
