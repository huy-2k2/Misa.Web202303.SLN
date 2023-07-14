using Misa.Web202303.QLTS.DL.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;
using LicenseModel = Misa.Web202303.QLTS.DL.Model.License;
namespace Misa.Web202303.QLTS.DL.Repository.License
{
    public interface ILicenseRepository : IBaseRepository<LicenseEntity>
    {
        /// <summary>
        /// lấy danh sách license Model (license và tài sản liên quan)
        /// created by: NQ Huy(28/06/2023)
        /// </summary>
        /// <param name="pageSize">kích thước trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa truy vấn</param>
        /// <returns>danh sách license Model</returns>
        Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string textSearch);

        
    }
}
