using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class BudgetDetailEntity : BaseEntity
    {
        public Guid budget_detail_id { get; set; }

        public Guid budget_id { get; set; }

        public Guid fixed_asset_id { get; set; }

        public double budget_value { get; set; }
    }
}
