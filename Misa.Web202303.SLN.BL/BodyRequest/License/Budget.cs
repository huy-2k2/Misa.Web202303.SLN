using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class Budget
    {
        public Guid budget_id { get; set; }

        public Guid? budget_detail_id { get; set; }

        public double budget_value { get; set; }
    }
}
