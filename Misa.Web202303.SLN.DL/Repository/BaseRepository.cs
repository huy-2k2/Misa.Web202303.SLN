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
        protected async Task<DbConnection> GetOpenConnectionAsync()
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
        public virtual async Task UpdateAsync(Guid entityId, TEntity entity)
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
        }


        /// <summary>
        /// thêm mới 1 bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>k
        /// <returns></returns>
        public virtual async Task InsertAsync(TEntity entity)
        {
            var connection = await GetOpenConnectionAsync();

            var tableName = this.GetTableName();
            // tạo lệnh sql

            var sql = $"INSERT INTO {tableName} (";

            var notNullProps = entity.GetType().GetProperties().Where(p => p.GetValue(entity) != null);
            sql += string.Join(", ", notNullProps.Select(p => p.Name));
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
        }

        /// <summary>
        /// kiểm tra  mã code đã tồn tại khi thêm hoạc sửa
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckCodeExistedAsync(string code, Guid? id)
        {
            var tableName = this.GetTableName();
            // tạo lệnh sql
            var sql = $"SELECT {tableName}_id FROM {tableName} WHERE {tableName}_code = @code AND (LENGTH(@id) = 0 OR {tableName}_id != @id)";

            var connection = await this.GetOpenConnectionAsync();
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("code", code);
            dynamicParams.Add("id", id == null ? "" : id);

            var entity = await connection.QueryFirstOrDefaultAsync(sql, dynamicParams);
            await connection.CloseAsync();
            return entity != null;
        }

        /// <summary>
        /// xóa nhiều bản ghi dựa vào chuỗi danh sách id, tách nhau bởi dấu ","
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public virtual async Task DeleteListAsync(string listId)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();

            var sql = $"DELETE FROM {tableName} WHERE FIND_IN_SET({tableName}_id, @list_id)";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("list_id", listId);

            await connection.ExecuteAsync(sql, dynamicParams);

            await connection.CloseAsync();

        }

        /// <summary>
        /// lấy ra tổng số bản ghi tồn tại trong danh sách chuỗi id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public virtual async Task<int> GetSumExistedOfListAsync(string listId)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();
            var dynamicParams = new DynamicParameters();
            var sql = $"SELECT COUNT(*) FROM {tableName} WHERE FIND_IN_SET({tableName}_id, @list_id)";
            dynamicParams.Add("list_id", listId);
            var result = await connection.QueryFirstAsync<int>(sql, dynamicParams);
            await connection.CloseAsync();
            return result;
        }

        /// <summary>
        /// thêm nhiều bản ghi cùng lúc, dùng cho khi import file
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public virtual async Task InsertListAsync(IEnumerable<TEntity> listEntity)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();
            var dynamicParams = new DynamicParameters();

            var sql = "";

            var index = 0;
            foreach (var entity in listEntity)
            {
                var notNullProps = entity.GetType().GetProperties().Where(prop => prop.GetValue(entity) != null);
                sql += $"INSERT INTO {tableName} (";
                sql += string.Join(", ", notNullProps.Select(prop => prop.Name));
                sql += ") Values (";
                sql += string.Join(", ", notNullProps.Select(prop => $"@{prop.Name}_{index}"));
                sql += ");";

                foreach (var prop in notNullProps)
                {
                    dynamicParams.Add($"{prop.Name}_{index}", prop.GetValue(entity));
                }

                index++;
            }

            await connection.ExecuteAsync(sql, dynamicParams);
            await connection.CloseAsync();

        }

       

        /// <summary>
        /// lấy dữ liệu import của table, column
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<ImportEntity>> GetImportDataAsync()
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();
            var sql = "SELECT import_column_index, prop_name, data_type, import_file_table, number_column " +
                "FROM import_file " +
                "JOIN import_column  ON import_file.import_file_id = import_column.import_file_id " +
                $"WHERE import_file_table = '{tableName}'";

            var result = await connection.QueryAsync<ImportEntity>(sql);
            await connection.CloseAsync();
            return result;
        }

        /// <summary>
        /// kiểm tra các mã code tồn tại trong listCode khi thêm nhiều tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<string>> GetListExistedCodeAsync(string listCode)
        {
            var connection = await GetOpenConnectionAsync();
            var tableName = GetTableName();
            var dynamicParams = new DynamicParameters();
            var sql = $"SELECT {tableName}_code FROM {tableName} WHERE FIND_IN_SET({tableName}_code, @list_code)";
            dynamicParams.Add("@list_code", listCode);
            
            var result = await connection.QueryAsync<string>(sql, dynamicParams);
            await connection.CloseAsync();
            return result;
        }

        /// <summary>
        /// lấy ra tên của table
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public abstract string GetTableName();

    }
}
