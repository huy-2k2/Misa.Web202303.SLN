using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.Department
{
    /// <summary>
    /// class dùng để trả dữ liệu về cho controller
    /// created by: nqhuy(21/05/2023)
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public class DepartmentImportDto
    {
        /// <summary>
        /// mã phòng ban
        /// </summary>
        [Required, Length(0, 50), NameAttribute(FieldName.DepartmentCode)]
        public string department_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        [Required, Length(0, 255), NameAttribute(FieldName.DepartmentName)]
        public string department_name { get; set; }
    }
}
