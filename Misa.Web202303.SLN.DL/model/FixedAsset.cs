using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Model
{
    public class FixedAsset
    {
        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// mã tài sản
        /// </summary>
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// tên tài sản
        /// </summary>
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
        public double cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao  mòn (%)
        /// </summary>
        public double depreciation_rate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        public double depreciation_annual { get; set; }                           

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        public int tracked_year { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        public int life_time { get; set; }

        /// <summary>
        /// id của license_detail tương ứng
        /// </summary>
        public Guid license_detail_id { get; set; }

        /// <summary>
        /// số thứ tự trong license
        /// </summary>
        public int license_detail_index { get; set; }   
    }
}
