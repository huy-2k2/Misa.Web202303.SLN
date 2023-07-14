using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.AuthRepository
{
    public interface IAuthRepository
    {
        /// <summary>
        /// lấy user dựa trên email và password
        /// created by:NQ huy (20/06/2023)
        /// </summary>
        /// <param name="email">email đăng nhập</param>
        /// <returns></returns>
        Task<User?> GetAuthAsync(string email);
    }
}
