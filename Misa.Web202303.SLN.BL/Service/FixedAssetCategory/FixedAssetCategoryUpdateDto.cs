using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Range = Misa.Web202303.QLTS.BL.ValidateDto.Attributes.Range;


namespace Misa.Web202303.QLTS.BL.Service.FixedAssetCategory
{
    /// <summary>
    /// class để nhận dữ liệu khi update loại tài sản, dùng ở controller và service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryUpdateDto
    {/// <summary>
     /// mã loại tài sản
     /// </summary>
        [Required, Length(0, 50), NameAttribute(FieldName.FixedAssetCategoryCode)]

        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        [Required, Length(0, 255), NameAttribute(FieldName.FixedAssetCategoryName)]
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// tỷ lệ hao mòn (%)
        /// </summary>
        [Higher(0), Lower(100), Name(FieldName.DepreciationRate)]
        public double depreciation_rate { get; set; }


        /// <summary>
        /// số năm sử dụng 
        /// </summary>
        [Range(1, 10000), NameAttribute(FieldName.LifeTime)]
        public int life_time { get; set; }
    }
}
