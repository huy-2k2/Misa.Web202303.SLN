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
            var sql = $"SELECT * FROM {this.GetTableName()} WHERE {this.GetTableName()}_id = @id";
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
            var sql = $"proc_update_{this.GetTableName()}";

            var dynamicParams = new DynamicParameters();

            // tên của param trong procedure giống với tên property
            foreach (var prop in entity.GetType().GetProperties())
            {
                dynamicParams.Add(prop.Name, prop.GetValue(entity));
            }

            dynamicParams.Add($"{this.GetTableName()}_id", entityId);

            await connection.ExecuteAsync(sql, dynamicParams, commandType: CommandType.StoredProcedure);
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
            var sql = $"DELETE FROM {this.GetTableName()} WHERE {this.GetTableName()}_id = @id";
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
            // tên của procedure
            var sql = $"proc_insert_{this.GetTableName()}";
            var dynamicParams = new DynamicParameters();
            // thêm các param (tên param giống với tên thuộc tính của FixedAsset)
            foreach (var prop in entity.GetType().GetProperties())
            {
                dynamicParams.Add(prop.Name, prop.GetValue(entity));
            }
            await connection.ExecuteAsync(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            await connection.CloseAsync();
            return entity;
        }


        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        protected abstract string GetTableName();
    }
}
