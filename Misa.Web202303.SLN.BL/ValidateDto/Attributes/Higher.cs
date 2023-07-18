using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Attributes
{
    /// <summary>
    /// đánh dấu giá trị nhỏ nhất
    /// </summary>
    public class Higher : BaseAttribute
    {
        /// <summary>
        /// giá trị nhỏ nhất
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (21/05/2023)
        /// </summary>
        /// <param name="min">giá trị nhỏ nhất</param>
        public Higher(double min)
        {
            Min = min;
        }   
    }
}
