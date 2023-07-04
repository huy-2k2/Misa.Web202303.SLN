using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseModel = Misa.Web202303.QLTS.DL.model.License;
namespace Misa.Web202303.QLTS.DL.filter
{
    public class FilterLicenseModel
    {
       /// <summary>
       /// danh sách license model
       /// </summary>
        public IEnumerable<LicenseModel> list_license_model { get; set; }

        /// <summary>
        /// tổng bản ghi
        /// </summary>
        public int total_license { get; set; }

        /// <summary>
        /// tổng nguyên giá tài sản tương ứng
        /// </summary>
        public double total_cost { get; set; }
    }
}
