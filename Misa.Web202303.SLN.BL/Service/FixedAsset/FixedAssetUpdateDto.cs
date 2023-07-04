using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Misa.Web202303.QLTS.BL.ValidateDto.Attributes.Range;


namespace Misa.Web202303.QLTS.BL.Service.FixedAsset
{
    /// <summary>
    /// dùng để nhận dữ liệu khi update, dùng ở controller và service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetUpdateDto
    { /// <summary>
      /// mã tài sản
      /// </summary>
        [Required, Length(0, 100), NameAttribute(FieldName.FixedAssetCode)]
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// tên tài sản
        /// </summary>
        /// 
        [Required, Length(0, 255), NameAttribute(FieldName.FixedAssetName)]
        public string fixed_asset_name { get; set; }

        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// id loại tài sản
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// ngày mua
        /// </summary>
        public DateTime purchase_date { get; set; }

        /// <summary>
        /// ngày sử dụng
        /// </summary>
        public DateTime use_date { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        [Higher(0), NameAttribute(FieldName.Cost)]
        public double cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        [Higher(1), NameAttribute(FieldName.Quantity)]
        public int quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao  mòn (%)
        /// </summary>
        [Higher(0), Lower(100), NameAttribute(FieldName.DepreciationRate)]
        public double depreciation_rate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        [Higher(0), NameAttribute(FieldName.DepreciationAnnual)]
        public double depreciation_annual { get; set; }

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        [CurrentYear, NameAttribute(FieldName.TrackedYear)]
        public int tracked_year { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        [Range(1, 10000), NameAttribute(FieldName.LifeTime)]
        public int life_time { get; set; }
    }
}
