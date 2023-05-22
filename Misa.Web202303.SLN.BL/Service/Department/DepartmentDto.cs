using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// class dùng để trả dữ liệu cho controller
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentDto
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
        public string Department_name { get; set; }
    }
}
