using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    public class FixedAssetExcel
    {
        /// <summary>
        /// mã tài sản
        /// </summary>
        public string Fixed_asset_code { get; set; }

        /// <summary>
        /// tên tài sản
        /// </summary>
        public string Fixed_asset_name { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string Department_name { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string Fixed_asset_category_name { get; set; }

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
        public double Cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao  mòn (%)
        /// </summary>
        public double Depreciation_rate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        public double Depreciation_annual { get; set; }

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        public int Tracked_year { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        public int Life_Time { get; set; }
    }
}
