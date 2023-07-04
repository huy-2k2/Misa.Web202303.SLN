using Misa.Web202303.QLTS.DL.filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.DL.Repository.FixedAsset
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của FixedAssetRepository, ngoài các phương thức của IBaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public interface IFixedAssetRepository : IBaseRepository<FixedAssetEntity>
    {

        /// <summary>
        /// filter, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="departmentId">mã phòng ban</param>
        /// <param name="fixedAssetCategoryId">mã loại tài sản</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns>danh sách tài sản thỏa mãn yêu cầu filter, phân trang</returns>
        Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch);

        /// <summary>
        /// lấy ra các tài sản chưa có chứng từ và chưa được chọn
        /// </summary>
        /// <param name="listIdSelected">danh sách id đã được chọn</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <param name="pageSize">kích thước của 1 page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <returns>danh sách tài sản</returns>
        Task<FilterListFixedAsset> GetFilterNotHasLicenseAsync(int pageSize, int currentPage, string listIdSelected, string? textSearch);

        /// <summary>
        /// lấy danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseId">id chứng từ</param>
        /// <returns>danh sách tài sản</returns>
        Task<IEnumerable<FixedAssetEntity>> GetListByLicenseId(Guid licenseId);
    }
}
