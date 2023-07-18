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
using BudgetModel = Misa.Web202303.QLTS.DL.Model.Budget;
using Misa.Web202303.QLTS.Common.Const;
using System.Data;
using Misa.Web202303.QLTS.DL.unitOfWork;

namespace Misa.Web202303.QLTS.DL.Repository.Budget
{
    public class BudgetRepository : BaseRepository<BudgetEntity>, IBudgetRepository
    {
        /// <summary>
        /// hàm khởi tạo
        /// created: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        public BudgetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// hàm lấy ra danh sách ngân sách của 1 tài sản
        /// created: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách ngân sách của 1 tài sản</returns>
        public async Task<IEnumerable<BudgetModel>> GetListBudgetModelAsync(Guid fixedAssetId)
        {
            var connection =  await GetOpenConnectionAsync();
            // lấy procedure
            var sql = ProcedureName.GET_LIST_BUDGET_MODEL;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_id", fixedAssetId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            // thực thi truy vấn
            var result = await connection.QueryAsync<BudgetModel, BudgetDetailEntity, BudgetModel>(sql: sql, param: dynamicParams,  transaction: transaction, commandType: CommandType.StoredProcedure, splitOn: "budget_detail_id", map: (budgetModel, budgetDetail) =>
            {
                budgetModel.budget_detail = budgetDetail;
                return budgetModel;
            });
            return result;
        }


        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// created by :NQ Huy(27/06/2023)
        /// </summary>
        /// <returns>tên của table budget</returns>
        public override string GetTableName()
        {
            return "budget";
        }
    }
}
