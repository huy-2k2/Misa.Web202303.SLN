using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Misa.Web202303.SLN.Common.Exceptions;


namespace Misa.Web202303.SLN.MiddleWares
{
    /// <summary>
    /// Middleware xử lý khi exception được bắn ra 
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// phương thức bắt exception và gọi hàm xử lý exception
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            } catch(Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// phương thức xử lý execption
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // kiểm tra excception có phải do lập trình viên throw hay không
            if (exception is BaseException)
            {
                // tạo message và trả về kết quả
                var ex = (BaseException)exception;
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new {
                            StatusCode = ex.StatusCode,
                            UserMessage = ex.UserMessage,
                            DevMessage = ex.DevMessage,
                        })
                );
            }
        }

    }
}
