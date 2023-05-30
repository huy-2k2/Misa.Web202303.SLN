using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    /// <summary>
    /// lớp  dùng để lấy dữ liệu khi phân trang
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FilterListFixedAsset
    {
        /// <summary>
        /// danh sách tài sản
        /// </summary>
        public IEnumerable<FixedAsset> List_fixed_asset { get; set; }
        
        /// <summary>
        /// sổ lượng tại sản
        /// </summary>
        public int Total_asset { get; set; }

        /// <summary>
        /// tổng số lượng tài sản
        /// </summary>
        public int Total_quantity { get; set; }

        /// <summary>
        /// tổng nguyên giá
        /// </summary>
        public double Total_cost { get; set; }
    }
}
