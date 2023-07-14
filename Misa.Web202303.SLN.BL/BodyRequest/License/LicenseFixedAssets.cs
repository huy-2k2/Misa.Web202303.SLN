using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class LicenseFixedAssets
    {
        public Guid fixed_asset_id { get; set; }

        public Guid? license_detail_id { get; set; }

        public IEnumerable<Budget>? budgets { get; set; }
    }
}
