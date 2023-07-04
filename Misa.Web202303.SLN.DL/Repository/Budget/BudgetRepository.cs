using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BudgetEntity = Misa.Web202303.QLTS.DL.Entity.Budget;
using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;
using BudgetModel = Misa.Web202303.QLTS.DL.model.Budget;
using Misa.Web202303.QLTS.Common.Const;
using System.Data;

namespace Misa.Web202303.QLTS.DL.Repository.Budget
{
    public class BudgetRepository : BaseRepository<BudgetEntity>, IBudgetRepository
    {
        /// <summary>
        /// Hàm khởi tạo
        /// created by :NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="configuration">configuration</param>
        public BudgetRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// hàm lấy ra danh sách ngân sách của 1 tài sản
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách ngân sách của 1 tài sản</returns>
        public async Task<IEnumerable<BudgetModel>> GetListBudgetModelAsync(Guid fixedAssetId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_LIST_BUDGET_MODEL;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_id", fixedAssetId);

            var result = await connection.QueryAsync<BudgetModel, BudgetDetailEntity, BudgetModel>(sql: sql, param: dynamicParams, commandType: CommandType.StoredProcedure, splitOn: "budget_detail_id", map: (budgetModel, budgetDetail) =>
            {
                budgetModel.budget_detail = budgetDetail;
                return budgetModel;
            });
            await connection.CloseAsync();
            return result;
        }

        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// created by :NQ Huy(27/06/2023)
        /// </summary>
        /// <returns>tên của table department</returns>
        public override string GetTableName()
        {
            return "budget";
        }
    }
}
