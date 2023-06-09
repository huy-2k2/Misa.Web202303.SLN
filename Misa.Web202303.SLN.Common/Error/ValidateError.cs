using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Error
{
    public class ValidateError
    {
        /// <summary>
        /// mô tả chi tiết lỗi
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// tên trường bị lỗi
        /// </summary>
        public string FieldNameError { set; get; }
    }
}
