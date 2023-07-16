using Dapper;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;

namespace Misa.Web202303.QLTS.DL.Repository.BudgetDetail
{
    public class BudgetDetailRepository : BaseRepository<BudgetDetailEntity>, IBudgetDetailRepository
    {
        public BudgetDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task DeleteByListLicenseDetailId(string listLicenseDetailId)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.DELETE_BUDGET_DETAIL_BY_LIST_LICENSE_DETAIL;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listLicenseDetailId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteListByListLicenseId(string listLicenseId)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.DELETE_BUDGET_DETAIL_BY_LIST_LICENSE;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listLicenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedBFAsync(string listBFId)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.GET_LIST_BUDGET_DETAIL_EXISTED_BY_BF;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_bf_id", listBFId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_LIST_BUDGET_DETAIL_EXISTED_OF_LICENSE;
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listId);
            dynamicParams.Add("license_id", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        public override string GetTableName()
        {
            return "budget_detail";
        }
    }
}
