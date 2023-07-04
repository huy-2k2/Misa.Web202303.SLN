using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class Budget:BaseEntity
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid budget_id { get; set; }

        /// <summary>
        /// tên nguồn ngân sách
        /// </summary>
        public string budget_name { get; set;}
    }
}
