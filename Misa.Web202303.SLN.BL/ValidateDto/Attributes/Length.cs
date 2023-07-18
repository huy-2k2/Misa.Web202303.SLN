using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Attributes
{
    /// <summary>
    /// đánh dấu độ dài chuỗi
    /// </summary>
    public class Length : BaseAttribute
    {
        /// <summary>
        /// độ dài lớn nhất
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// độ dài nhỏ nhất
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (21/05/2023)
        /// </summary>
        /// <param name="min">độ dài nhỏ nhất</param>
        /// <param name="max">độ dài lớn nhất</param>
        public Length(int min, int max)
        {
            Max = max;
            Min = min;
        }
    }
}
