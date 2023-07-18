using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Model
{
    public class License
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid license_id { get; set; }

        /// <summary>
        /// mã code
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

        /// <summary>
        /// tổng tiền của 1 chứng từ
        /// </summary>
        public double cost { set; get; }
    }
}
