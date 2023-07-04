using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.ImportService;
using Misa.Web202303.QLTS.DL.filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEnitty = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.Service.FixedAsset
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của FixedAssetService, ngoài các phương thức của IBaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public interface IFixedAssetService : IBaseService<FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>
    {
        /// <summary>
        /// tự động sinh mã tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>mã tài sản gợi ý khi thêm mới</returns>
        Task<string> GetRecommendFixedAssetCodeAsync();

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
        /// import dữ liệu tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream">file import dưới dạng stream</param>
        /// <param name="isSubmit">biến kiểm tra người dùng có đang submit</param>
        /// <returns>dữ liệu về file excel và dữ liệu valdiate</returns>
        public Task<ImportErrorEntity<FixedAssetEnitty>> ImportFileAsync(MemoryStream stream, bool isSubmit);


       
        /// <summary>
        ///  lấy danh sách tài sản thoải mãn điều kiện filter và phân trang, chưa có chứng từ
        /// </summary>
        /// <param name="filterFixedAssetNoLicense">đối tượng chứa dữ liệu filter</param>
        /// <returns>danh sách tài sản chưa có chứng từ thỏa mã điều kiện filter</returns>
        public Task<FilterListFixedAsset> GetFilterNotHasLicenseAsync(FilterFixedAssetNoLicense filterFixedAssetNoLicense);

        public Task<IEnumerable<FixedAssetDto>> GetListByLicenseIdAsync(Guid licenseId);
    }
}
