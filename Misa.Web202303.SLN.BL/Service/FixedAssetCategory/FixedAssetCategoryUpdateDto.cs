using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Range = Misa.Web202303.SLN.BL.ValidateDto.Attributes.Range;


namespace Misa.Web202303.SLN.BL.Service.FixedAssetCategory
{
    /// <summary>
    /// class để nhận dữ liệu khi update loại tài sản, dùng ở controller và service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryUpdateDto
    {
        /// <summary>
        /// mã loại tài sản
        /// </summary>
        [Length(0, 50), Required, NameAttribute("mã loại tài sản")]

        public string Fixed_asset_category_code { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        [Length(0, 255), Required, NameAttribute("tên loại tài sản")]
        public string Fixed_asset_category_name { get; set; }

        /// <summary>
        /// tỷ lệ hao mòn (%)
        /// </summary>
        [Range(0.0001, 100), NameAttribute("tệ lệ hao mòn")]
        public double Depreciation_rate { get; set; }

        /// <summary>
        /// số năm sử dụng 
        /// </summary>
        [Range(1, int.MaxValue), NameAttribute("thời gian sử dụng")]
        public int Life_time { get; set; }
    }
}
