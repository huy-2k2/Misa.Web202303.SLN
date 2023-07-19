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
        /// <summary>
        /// validate khi filter tài sản ở trang tài sản
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="pageSize">kích thước page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <param name="departmentId">id phòng ban</param>
        /// <param name="fixedAssetCategoryId">id của loại tài sản</param>
        /// <returns></returns>
        Task ValidateInputFilterAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId);

        /// <summary>
        /// hàm validate khi create
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetCreateDto">đối tượng FixedAssetCreateDto</param>
        /// <returns></returns>
        Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto);

        /// <summary>
        /// hàm valdiate khi update 
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetId">id của tài sản</param>
        /// <param name="fixedAssetUpdateDto">đối tượng FixedAssetUpdateDto</param>
        /// <returns></returns>
        Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto);

        /// <summary>
        /// hàm validate khi filter lúc chọn tài sản cho chứng từ
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="input">đối tượng chứa dữ liệu đầu vào để filter</param>
        /// <returns></returns>
        Task GetFilterNoLicenseValidateAsync(FilterFixedAssetNoLicense input);

        /// <summary>
        /// hàm validate khí xóa nhiều
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listId">danh sách id xóa</param>
        /// <returns></returns>
        Task DeleteListValidateAsync(IEnumerable<Guid> listId);

        /// <summary>
        /// hàm validate business
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAsset">entitty FixedAsset</param>
        /// <returns>danh sách lỗi</returns>
        List<ValidateError> BusinessValidate(FixedAssetEntity fixedAsset);
    }
}
