using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    /// <summary>
    /// lớp nhận dữ liệu phòng ban từ db
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid Department_id { get; set; }

        /// <summary>
        /// mã phòng ban
        /// </summary>
        public string Department_code { get; set; } 
    
       /// <summary>
       /// tên phòng ban
       /// </summary>
        public string Department_name { get;set; }
    }
}
