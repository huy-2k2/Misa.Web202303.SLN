using Dapper;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
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

            var tableName = GetTableName();

            var sql = $"DELETE {tableName} FROM {tableName}" +
                $" JOIN license_detail on {tableName}.fixed_asset_id = license_detail.fixed_asset_id " +
                $"WHERE FIND_IN_SET(license_detail.license_detail_id, @listLicenseDetailId) != 0";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("listLicenseDetailId", listLicenseDetailId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction);
        }

        public async Task DeleteListByListLicenseId(string listLicenseId)
        {
            var connection = await GetOpenConnectionAsync();

            var tableName = GetTableName();

            var sql = $"DELETE {tableName} FROM {tableName}" +
                $" JOIN fixed_asset on fixed_asset.fixed_asset_id = {tableName}.fixed_asset_id " +
                $"JOIN license_detail on license_detail.fixed_asset_id = fixed_asset.fixed_asset_id " +
                $"WHERE FIND_IN_SET(license_detail.license_id, @listLicenseId) != 0";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("listLicenseId", listLicenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction);
        }

        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedBFAsync(string listBFId)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();

            var sql = $"SELECT * FROM {tableName} WHERE FIND_IN_SET(CONCAT(budget_id,'.', fixed_asset_id), @listBFId) != 0";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("listBFId", listBFId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction);
            return result;
        }

        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId)
        {
            var connection = await GetOpenConnectionAsync();
            var tableName = GetTableName();
            var sql = $"SELECT * FROM {tableName} JOIN license_detail ON license_detail.fixed_asset_id = budget_detail.fixed_asset_id WHERE license_id = @licenseId AND FIND_IN_SET({tableName}_id, @listId) != 0";

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("listId", listId);
            dynamicParams.Add("licenseId", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction);
            return result;
        }

        public override string GetTableName()
        {
            return "budget_detail";
        }
    }
}
