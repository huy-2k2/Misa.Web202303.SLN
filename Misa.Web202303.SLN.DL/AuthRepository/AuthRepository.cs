using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        #region
        /// <summary>
        /// connectionstring
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (20/06/2023)
        /// </summary>
        /// <param name="configuration">configuration</param>
        public AuthRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// hàm lấy dữ lệu người dùng dựa trên email và mật khẩu, nếu không tồn tại trả về null
        /// created by: NQ Huy (20/06/2023)
        /// </summary>
        /// <param name="email">email đăng nhập</param>
        /// <returns>dữ liệu người dùng</returns>
        public async Task<User?> GetAuthAsync(string email)
        {
            var connection = await _unitOfWork.GetDbConnectionAsync();

            var sql = "SELECT * FROM user WHERE BINARY email = @email";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("email", email);

            var result = await connection.QueryFirstOrDefaultAsync<User>(sql, dynamicParams);

            return result;
        }
        #endregion  
    }
}
