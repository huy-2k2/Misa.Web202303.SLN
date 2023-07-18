using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class LicenseFixedAssets
    {
        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// id chi tiết chứng từ
        /// </summary>
        public Guid? license_detail_id { get; set; }

        /// <summary>
        /// danh sách budget của tài sản
        /// </summary>
        public IEnumerable<Budget>? budgets { get; set; }
    }
}
