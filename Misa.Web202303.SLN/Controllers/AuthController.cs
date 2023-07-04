using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.BL.AuthService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace Misa.Web202303.QLTS.API.Controllers
{
    /// <summary>
    /// controller để đăng nhập, đăng ký
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// IAuthService
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(20/06/2023)
        /// </summary>
        /// <param name="authService">authService</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// endpoint lấy token
        /// created by: NQ Huy(20/06/2023)
        /// </summary>
        /// <param name="auth">AuthDto</param>
        /// <returns>bearer token</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(AuthDto auth)
        {
            var token = await _authService.GetAuthAsync(auth);


            return Ok(token);
        }

        /// <summary>
        /// endpoint kiểm tra người dùng đã có mã token hợp lệ chưa, nếu rồi thì trả về true, không thì throw exception
        /// created by: NQ Huy(20/06/2023)
        /// </summary>
        /// <returns>true nếu đã có token hợp lệ</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CheckLogined()
        {
            return Ok(true);
        }
    }
}
