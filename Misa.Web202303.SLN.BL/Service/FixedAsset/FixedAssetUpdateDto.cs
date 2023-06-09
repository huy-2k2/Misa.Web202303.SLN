using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Misa.Web202303.SLN.BL.ValidateDto.Attributes.Range;


namespace Misa.Web202303.SLN.BL.Service.FixedAsset
{
    /// <summary>
    /// dùng để nhận dữ liệu khi update, dùng ở controller và service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetUpdateDto
    {
        /// <summary>
        /// mã tài sản
        /// </summary>
        [Required, Length(0, 100),  NameAttribute(FieldName.FixedAssetCode)]
        public string Fixed_asset_code { get; set; }

        /// <summary>
        /// tên tài sản
        /// </summary>
        /// 
        [Required, Length(0, 255) , NameAttribute(FieldName.FixedAssetName)]
        public string Fixed_asset_name { get; set; }

        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid Department_id { get; set; }

        /// <summary>
        /// id loại tài sản
        /// </summary>
        public Guid Fixed_asset_category_id { get; set; }

        /// <summary>
        /// ngày mua
        /// </summary>
        public DateTime Purchase_date { get; set; }

        /// <summary>
        /// ngày sử dụng
        /// </summary>
        public DateTime Use_date { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        [Range(0, double.MaxValue), NameAttribute(FieldName.Cost)]
        public double Cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        [Range(1, int.MaxValue), NameAttribute(FieldName.Quantity)]
        public int Quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao  mòn (%)
        /// </summary>
        [Range(0.0001, 100), NameAttribute(FieldName.DepreciationRate)]
        public double Depreciation_rate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        [Range(0, double.MaxValue), NameAttribute(FieldName.DepreciationAnnual)]
        public double Depreciation_annual { get; set; }

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        [CurrentYear, NameAttribute(FieldName.TrackedYear)]
        public int Tracked_year { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        [Range(1, int.MaxValue), NameAttribute(FieldName.LifeTime)]
        public int Life_Time { get; set; }
    }
}
