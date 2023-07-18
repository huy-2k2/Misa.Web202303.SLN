using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class Budget
    {
        /// <summary>
        /// id nguồn hình thành
        /// </summary>
        public Guid budget_id { get; set; }

        /// <summary>
        /// id chi tiết nguồn hình thành
        /// </summary>
        public Guid? budget_detail_id { get; set; }

        /// <summary>
        /// giá trị nguồn hình thành
        /// </summary>
        public double budget_value { get; set; }

        /// <summary>
        /// cờ xem người dùng đã thay đổi vào nguồn hình thành hay chưa
        /// </summary>
        public bool? is_changed { get; set; }
    }
}
