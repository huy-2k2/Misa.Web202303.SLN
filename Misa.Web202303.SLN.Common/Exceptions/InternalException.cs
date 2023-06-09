using Misa.Web202303.SLN.Common.Emum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Misa.Web202303.SLN.Common.Exceptions
{
    /// <summary>
    /// exception do lỗi hệ thống
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class InternalException : BaseException
    {
        /// <summary>
        /// hàm khởi tạo, mặc định mã lỗi InternalServerError
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        public InternalException() {
            HttpStatusCode = HttpStatusCode.InternalServerError;
       }
    }
}
