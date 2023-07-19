using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Misa.Web202303.QLTS.DL.unitOfWork
{
    /// <summary>
    /// lấy, tạo connection và transaction để làm việc với database
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region
        /// <summary>
        /// connection string để tạo connect đến db
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// DbConnection
        /// </summary>
        private DbConnection _dbConnection;

        /// <summary>
        /// DbTransaction
        /// </summary>
        private DbTransaction _dbTransaction;
        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by : NQ Huy (05/07/2023)
        /// </summary>
        /// <param name="configuration">configuration<param>
        public UnitOfWork(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"] ?? "";
        }

        /// <summary>
        /// lấy ra connection cho 1 request
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns>connection</returns>
        public DbConnection GetDbConnection()
        {
            _dbConnection ??= new MySqlConnector.MySqlConnection(_connectionString);

            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }

            return _dbConnection;
        }

        /// <summary>
        /// lấy ra transaction cho 1 request
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns>transaction</returns>
        public DbTransaction GetTransaction()
        {
            return _dbTransaction ??= GetDbConnection().BeginTransaction();
        }

        // <summary>
        /// rollback khi xảy ra exception
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns></returns>
        public void Rollback()
        {
            _dbTransaction.Rollback();
            SaveChange();
        }

        // <summary>
        /// commit transaction
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns></returns>
        public void Commit()
        {
            if (_dbTransaction != null)
            {
                _dbTransaction.Commit();
            }
            SaveChange();
        }

        /// <summary>
        /// lấy ra connection cho 1 request
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns>connection</returns>
        public async Task<DbConnection> GetDbConnectionAsync()
        {
            _dbConnection ??= new MySqlConnector.MySqlConnection(_connectionString);

            if (_dbConnection.State == ConnectionState.Closed)
                await _dbConnection.OpenAsync();

            return _dbConnection;
        }

        /// <summary>
        /// lấy ra transaction cho 1 request
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns>transaction</returns>
        public async Task<DbTransaction> GetTransactionAsync()
        {
            _dbConnection ??= await GetDbConnectionAsync();
            if (_dbConnection.State == ConnectionState.Closed)
                await _dbConnection.OpenAsync();
            _dbTransaction ??= await _dbConnection.BeginTransactionAsync();
            return _dbTransaction;
        }

        // <summary>
        /// rollback khi xảy ra exception
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            await _dbTransaction.RollbackAsync();
            await SaveChangeAsync();
        }

        /// <summary>
        /// commit transaction
        /// created by: NQ Huy (07/07/2023)
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            if (_dbTransaction != null)
            {
                await _dbTransaction.CommitAsync();
            }
            await SaveChangeAsync();
        }

        /// <summary>
        /// đóng kết nối
        /// created by: NQ Huy(07/07/2023)
        /// </summary>
        /// <returns></returns>
        public void SaveChange()
        {
            if (_dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }

        /// <summary>
        /// đóng kết nối
        /// created by: NQ Huy(07/07/2023)
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangeAsync()
        {
            await _dbConnection.CloseAsync();
            if (_dbTransaction != null)
            {
                await _dbTransaction.DisposeAsync();
            }
        }
        #endregion
    }
}
