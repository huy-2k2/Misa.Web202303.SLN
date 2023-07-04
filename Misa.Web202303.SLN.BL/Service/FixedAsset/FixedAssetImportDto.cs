using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;
using Range = Misa.Web202303.QLTS.BL.ValidateDto.Attributes.Range;

namespace Misa.Web202303.QLTS.BL.Service.Dto
{
    /// <summary>
    /// dto dùng cho việc nhận dữ liệu import từ file excel
    /// </summary>
    public class FixedAssetImportDto
    {
        /// <summary>
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
        [Required, Length(0, 50), NameAttribute(FieldName.DepartmentCode)]
        public string department_code { get; set; }

        /// <summary>
        /// id loại tài sản
        /// </summary>
        [Required, Length(0, 50), NameAttribute(FieldName.FixedAssetCategoryCode)]
        public string fixed_asset_category_code { get; set; }

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
        [Range(1, 10000), NameAttribute(FieldName.LifeTime)]
        public int life_time { get; set; }
    }
}
