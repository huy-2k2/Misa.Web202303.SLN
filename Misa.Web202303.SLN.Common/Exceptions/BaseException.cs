using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Exceptions
{
    /// <summary>
    /// các exception chủ động throw sẽ kế thừa từ BaseException
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// mã lỗi
        /// </summary>
        public int StatusCode { get; set; } 

        /// <summary>
        /// lỗi thông báo cho người dùng
        /// </summary>
        public string? UserMessage { get; set; }

        /// <summary>
        /// lỗi thông báo cho lập trình viên
        /// </summary>
        public string? DevMessage { get; set; }
    }
}
