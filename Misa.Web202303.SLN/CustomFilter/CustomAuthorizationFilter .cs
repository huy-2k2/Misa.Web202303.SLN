using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Resource;
using System.Net;

namespace Misa.Web202303.QLTS.API.CustomFilter
{
    public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        /// AuthorizationPolicy
        /// </summary>
        public AuthorizationPolicy Policy { get; }

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(20/06/2023)
        /// </summary>
        public CustomAuthorizationFilter()
        {
            Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        }

        /// <summary>
        /// kiểm tra nếu mã lỗi là 401 thì custom lại response
        /// created by: NQ Huy(20/06/2023)
        /// </summary>
        /// <param name="context">context</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // cho phép các endpoint anonymous skip qua authori
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
            {
                throw new ValidateException() 
                {
                    HttpStatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = ErrorCode.InvalidToken,
                    UserMessage = ErrorMessage.TokenInvalidError
                };
            }
        }
    }
}
