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
    /// exception khi không tìm thấy tài nguyên
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class NotFoundException : BaseException
    {
        /// <summary>
        /// phương thức khởi tạo, mẵ định mã lỗi là NotFound
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        public NotFoundException() {
            HttpStatusCode = HttpStatusCode.NotFound;
        }
    }
}
