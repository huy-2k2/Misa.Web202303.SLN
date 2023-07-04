using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.model
{
    public class Budget :BaseModel
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid budget_id { get; set; }

        /// <summary>
        /// tên nguồn ngân sách
        /// </summary>
        public string budget_name { get; set; }

        /// <summary>
        /// Budget Detail
        /// </summary>
        public BudgetDetail budget_detail { get; set; }

    }
}
