using Misa.Web202303.SLN.Common.Emum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        /// mã lỗi http
        /// </summary>
        public HttpStatusCode  HttpStatusCode { get; set; }
        
        /// <summary>
        /// mã lỗi quy định riêng
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// dữ liệu về lỗi
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// thông báo lỗi
        /// </summary>
        public string UserMessage { get; set; }
    }
}
