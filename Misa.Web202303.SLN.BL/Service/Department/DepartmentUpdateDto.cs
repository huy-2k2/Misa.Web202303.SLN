using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// class dùng để nhận dữ liệu khi sửa
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentUpdateDto
    {
        /// <summary>
        /// mã phòng ban
        /// </summary>
        [Required, Length(0, 50), NameAttribute(FieldName.DepartmentCode)]
        public string Department_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        [Required, Length(0, 255), NameAttribute(FieldName.DepartmentName)]
        public string Department_name { get; set; }
    }
}
