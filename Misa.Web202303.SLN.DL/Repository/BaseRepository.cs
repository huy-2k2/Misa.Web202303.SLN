using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Repository
{
    /// <summary>
    /// thực thi các phương thức của IBaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        private readonly string _connectionString;

        /// <summary>
        /// hàm khởi tạo, lấy IConfiguration từ dependency injection
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration"></param>
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"] ?? "";
        }

        /// <summary>
        /// mở kết nối
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = new MySqlConnector.MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// lấy 1 bản ghi theo id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            var connection = await GetOpenConnectionAsync();
            var tableName = this.GetTableName();
            var sql = $"SELECT * FROM {tableName} WHERE {tableName}_id = @id";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("id", id);

            var result = await connection.QueryFirstOrDefaultAsync<TEntity>(sql, dynamicParams);

            await connection.CloseAsync();
            return result;
        }

        /// <summary>
        /// lấy tất cả bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            var connection = await GetOpenConnectionAsync();
            var sql = $"SELECT * FROM {this.GetTableName()}";

            var result = await connection.QueryAsync<TEntity>(sql);
            await connection.CloseAsync();

            return result;
        }

        /// <summary>
        /// cập nhật 1 bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(Guid entityId, TEntity entity)
        {
            var connection = await GetOpenConnectionAsync();
            // tên của procedure
            //var sql = $"proc_update_{this.GetTableName()}";
            var tableName = this.GetTableName();
            // khởi tạo câu lệnh sql
            var sql = $"UPDATE {tableName} SET "; 
            var notNullProps = entity.GetType().GetProperties().Where(p => p.GetValue(entity) != null);
            sql += string.Join(", ", notNullProps.Select(p => $"{p.Name} = @{p.Name}"));
            sql += $" WHERE {tableName}_id = @{tableName}_id";

            var dynamicParams = new DynamicParameters();

            // tên của param trong procedure giống với tên property
            foreach (var prop in notNullProps)
            {
                dynamicParams.Add(prop.Name, prop.GetValue(entity));
            }

            dynamicParams.Add($"{tableName}_id", entityId);

            await connection.ExecuteAsync(sql, dynamicParams);
            await connection.CloseAsync();
            return entity;
        }

        /// <summary>
        /// xóa một bản ghi theo id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var connection = await GetOpenConnectionAsync();
            var tableName = this.GetTableName();
            var sql = $"DELETE FROM {tableName} WHERE {tableName}_id = @id";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("id", id);
            await connection.ExecuteAsync(sql, dynamicParams);
            await connection.CloseAsync();
            return true;
        }

        /// <summary>
        /// thêm mới 1 bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>k
        /// <returns></returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var connection = await GetOpenConnectionAsync();

            var tableName = this.GetTableName();
            // tạo lệnh sql
        
            var sql = $"INSERT INTO {tableName} (";

            var notNullProps = entity.GetType().GetProperties().Where(p => p.GetValue(entity) != null);
            sql += string.Join(", ", notNullProps.Select(p =>  p.Name));
            sql += ") VALUES (";
            sql += string.Join(", ", notNullProps.Select(p => $"@{p.Name}"));
            sql += ")";
            var dynamicParams = new DynamicParameters();
            // thêm các param (tên param giống với tên thuộc tính của FixedAsset)
            foreach (var prop in notNullProps)
            {
                dynamicParams.Add(prop.Name, prop.GetValue(entity));
            }
            dynamicParams.Add($"{tableName}_id", Guid.NewGuid());
            await connection.ExecuteAsync(sql, dynamicParams);
            await connection.CloseAsync();
            return entity;
        }

        /// <summary>
        /// kiểm tra  mã code đã tồn tại khi thêm hoạc sửa
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckCodeExistedAsync(string code, Guid? id)
        {
            var tableName = this.GetTableName();
            // tạo lệnh sql
            var sql = $"SELECT {tableName}_id FROM {tableName} WHERE {tableName}_code = @code AND (LENGTH(@id) = 0 OR {tableName}_id != @id)";

            var connection = await this.GetOpenConnectionAsync();
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("code", code);
            dynamicParams.Add("id", id == null? "" : id);

            var entity = await connection.QueryFirstOrDefaultAsync(sql, dynamicParams);
            await connection.CloseAsync();
            return entity != null;
        }

        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        protected abstract string GetTableName();
    }
}
