using Misa.Web202303.QLTS.BL.BodyRequest.License;
using Misa.Web202303.QLTS.DL.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseModel = Misa.Web202303.QLTS.DL.Model.License;

namespace Misa.Web202303.QLTS.BL.Service.License
{
    public interface ILicenseService : IBaseService<LicenseDto, LicenseUpdateDto, LicenseCreateDto>
    {
        /// <summary>
        /// phân trang, Filter cho chứng từ
        /// </summary>
        /// <param name="pageSize">kích thước 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns>danh chứng license Model thỏa mãn điều kiện phân trang, Filter</returns>
        Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string? textSearch);

        /// <summary>
        /// hàm lấy mã gợi ý cho chứng từ
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <returns>mã code gợi ý</returns>
        Task<string> GetRecommendAsync();

        Task InsertModelAsync(CULicense cuLicense);

        Task UpdateModelAsync(Guid licenseId, CULicense cuLicense);

    }
}
