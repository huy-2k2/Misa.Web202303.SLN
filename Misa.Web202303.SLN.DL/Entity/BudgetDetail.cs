using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class BudgetDetail
    {
        /// <summary>
        /// id budget detail
        /// </summary>
        public Guid budget_detail_id { get; set; }

        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// giá trị ngân sách
        /// </summary>
        public double budget_value { get; set; }    

        public Guid budget_id { get; set; }
    }
}
