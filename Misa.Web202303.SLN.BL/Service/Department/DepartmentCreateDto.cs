using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.Department
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
        [Required, Length(0, 50),  NameAttribute(FieldName.DepartmentCode)]
        public string department_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        [Required, Length(0, 255),  NameAttribute(FieldName.DepartmentName)]
        public string department_name { get; set; }
    }
}
