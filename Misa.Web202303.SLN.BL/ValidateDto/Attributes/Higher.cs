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

        public Higher(double min)
        {
            Min = min;
        }   
    }
}
