using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.License
{
    public class LicenseCreateDto
    {
        /// <summary>
        /// mã chứng từ
        /// </summary>

        [Required, Length(0, 100), NameAttribute(FieldName.LicenseCode)]
        public string license_code { get; set; }

        /// <summary>
        /// ngày chứng từ
        /// </summary>
        public DateTime create_day { get; set; }

        /// <summary>
        /// ngày ghi tăng
        /// </summary>
        public DateTime use_day { get; set; }

        /// <summary>
        /// nôi dung
        /// </summary>
        public string? content { get; set; }
    }
}
