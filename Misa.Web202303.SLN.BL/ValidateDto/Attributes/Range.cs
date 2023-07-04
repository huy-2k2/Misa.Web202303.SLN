using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Attributes
{
    /// <summary>
    /// đánh dấu giá trị nằm trong khoảng
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class Range : BaseAttribute
    {
        /// <summary>
        /// giá trị nhỏ nhất
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// giá trị lớn nhất
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// hàm khởi tạo
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(double min, double max) {
            Min = min; Max = max;
        }
    }
}
