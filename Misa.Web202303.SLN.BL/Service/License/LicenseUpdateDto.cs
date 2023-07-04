using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.License
{
    public class LicenseUpdateDto
    { 
        /// <summary>
        /// mã chứng từ
        /// </summary>
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
        public string content { get; set; }
    }
}
