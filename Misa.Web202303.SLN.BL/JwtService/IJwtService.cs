using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.JwtService
{
    public interface IJwtService
    {
        /// <summary>
        /// tạo token để xác thực người dùng
        /// </summary>
        /// <param name="user">thông tin người dùng</param>
        /// <returns>token</returns>
        string CreateToken(User user);
        
    }
}
