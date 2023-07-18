using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class LicenseDetail : BaseEntity
    {
        public Guid license_detail_id { set; get; }

        public Guid license_id { set; get; }

        public Guid fixed_asset_id { set; get; }

        public int license_detail_index { set; get; }

    }
}
