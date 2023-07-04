using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misa.Web202303.QLTS.DL.Entity;

namespace Misa.Web202303.QLTS.DL.filter
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
        public IEnumerable<FixedAsset> list_fixed_asset { get; set; }

        /// <summary>
        /// sổ lượng tại sản
        /// </summary>
        public int total_asset { get; set; }

        /// <summary>
        /// tổng số lượng tài sản
        /// </summary>
        public int total_quantity { get; set; }

        /// <summary>
        /// tổng nguyên giá
        /// </summary>
        public double total_cost { get; set; }
    }
}
