using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// constructor
        /// created by: Nguyen Quoc Huy(27/06/2023)
        /// </summary>
        /// <param name="configuration">configuration</param>
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// tạo token để xác thực người dùng
        /// </summary>
        /// <param name="user">thông tin người dùng</param>
        /// <returns>token được mã hóa từ thông tin người dùng</returns>
        public string CreateToken(User user)
        {
            var claims = new[] {
                        new Claim(ClaimTypes.Email, $"{user.email}"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:LifeTime"])),
                        signingCredentials: signIn
                        );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
