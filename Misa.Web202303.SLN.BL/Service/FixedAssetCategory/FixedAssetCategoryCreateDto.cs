using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service.FixedAssetCategory
{
    /// <summary>
    /// class dùng để nhận dữ liệu khi tạo mới, dùng ở service và controller
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryCreateDto
    {
        /// <summary>
        /// mã loại tài sản
        /// </summary>
        public string Fixed_asset_category_code { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string Fixed_asset_category_name { get; set; }

        /// <summary>
        /// tỷ lệ hao mòn (%)
        /// </summary>
        public double Depreciation_rate { get; set; }

        /// <summary>
        /// số năm sử dụng 
        /// </summary>
        public int Life_time { get; set; }
    }
}
