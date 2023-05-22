using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    /// <summary>
    /// lớp nhận dữ liệu loại tài sản từ db
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FixedAssetCategory : BaseEntity
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
        public string Fixed_asset_category_name { get;set; }

        /// <summary>
        /// tỷ lệ hao mòn (%)
        /// </summary>
        public  double Depreciation_rate { get; set; }

        /// <summary>
        /// số năm sử dụng 
        /// </summary>
        public int Life_time { get; set; }
    }
}
