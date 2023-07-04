using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest
{
    /// <summary>
    /// đối tượng để lấy dữ liệu fontend khi filter, phân trang tài sản chưa có chứng từ
    /// created by :NQ Huy(28/06/2023)
    /// </summary>
    public class FilterFixedAssetNoLicense
    {
        /// <summary>
        /// kích thước page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// page hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// danh sách id tài sản đã được chọn
        /// </summary>
        public IEnumerable<Guid> ListIdSelected { set; get; }

        /// <summary>
        /// từ khóa tìm kiếm
        /// </summary>
        public string? TextSearch { get; set; }
    }

}
