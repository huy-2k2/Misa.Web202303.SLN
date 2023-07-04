using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
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
        private readonly string _connectionString;
        #endregion


        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (20/06/2023)
        /// </summary>
        /// <param name="configuration">configuration</param>
        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"] ?? "";
        }

        /// <summary>
        /// hàm mở kết nối
        /// created by : NQ Huy(20/06/2023)
        /// </summary>
        /// <returns>đối tượng để kết nối tới database</returns>
        private async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = new MySqlConnector.MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// hàm lấy dữ lệu người dùng dựa trên email và mật khẩu, nếu không tồn tại trả về null
        /// created by: NQ Huy (20/06/2023)
        /// </summary>
        /// <param name="email">email đăng nhập</param>
        /// <param name="password">mật khẩu</param>
        /// <returns>dữ liệu người dùng</returns>
        public async Task<User?> GetAuthAsync(string email, string password)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = "SELECT * FROM user WHERE BINARY email = @email AND password = @password";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("email", email);
            dynamicParams.Add("password", password);

            var result = await connection.QueryFirstOrDefaultAsync<User>(sql, dynamicParams);
            
            await connection.CloseAsync();

            return result;
        }
        #endregion  
    }
}
