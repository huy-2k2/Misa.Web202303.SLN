using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// class dùng để nhân khi liệu khi tạo mới
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentCreateDto
    {
        /// <summary>
        /// mã phòng ban
        /// </summary>
        [Length(0, 50), Required, NameAttribute("mã bộ phận sử dụng")]
        public string Department_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        [Length(0, 255), Required, NameAttribute("tên bộ phận sử dụng")]
        public string Department_name { get; set; }
    }
}
