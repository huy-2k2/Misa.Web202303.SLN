using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Misa.Web202303.QLTS.DL.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private string _connectionString;

        private static DbConnection _dbConnection;
        
        private static DbTransaction _dbTransaction;

        public UnitOfWork(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"] ?? "";
        }

        public DbConnection GetDbConnection()
        {
            if(_dbConnection == null )
            {
                _dbConnection = new MySqlConnector.MySqlConnection(_connectionString);
            }
            _dbConnection.Open();
            return _dbConnection;
        }

        public DbTransaction GetTransaction()
        {
            if(_dbTransaction == null )
            {
                _dbConnection ??= GetDbConnection();
                _dbTransaction = _dbConnection.BeginTransaction();
            }
            return _dbTransaction;
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
            _dbConnection.Close();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            _dbConnection.Close();
        }
    }
}
