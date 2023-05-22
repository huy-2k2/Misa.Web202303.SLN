using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service.FixedAssetCategory
{
    /// <summary>
    /// class dùng để trả dữ liệu về cho controller
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryDto
    {
        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid Fixed_asset_category_id { get; set; }

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
