using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class CULicense
    {
        public LicenseCUDto license { get; set; }

        public IEnumerable<LicenseFixedAssets> list_fixed_asset { get; set; }

        public IEnumerable<Guid> list_fixed_asset_id_delete { get; set; }

        public IEnumerable<Guid> list_budget_detail_id_delete { get; set; }
    }
}
