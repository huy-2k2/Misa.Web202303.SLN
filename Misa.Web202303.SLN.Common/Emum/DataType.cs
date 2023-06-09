using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Emum
{
    public enum DataType
    {
        /// <summary>
        /// kiểu chuỗi
        /// </summary>
        StringType = 0,

        /// <summary>
        /// kiểu Guid
        /// </summary>
        GuidType = 1,

        /// <summary>
        /// kiểu số nguyên
        /// </summary>
        IntType = 2,

        /// <summary>
        /// kiểu số thực
        /// </summary>
        DoubleType = 3,

        /// <summary>
        /// kiểu thời gian
        /// </summary>
        DateTimeType = 4,

        /// <summary>
        /// kiểu năm 
        /// </summary>
        YearType = 5,
    }
}
