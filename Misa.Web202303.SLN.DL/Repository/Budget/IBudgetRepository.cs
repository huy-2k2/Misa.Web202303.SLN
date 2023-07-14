using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BudgetEntity = Misa.Web202303.QLTS.DL.Entity.Budget;
using BudgetModel = Misa.Web202303.QLTS.DL.Model.Budget;

namespace Misa.Web202303.QLTS.DL.Repository.Budget
{
    public interface IBudgetRepository : IBaseRepository<BudgetEntity>
    {
        /// <summary>
        /// hàm lấy ra danh sách ngân sách của 1 tài sản
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách ngân sách của 1 tài sản</returns>
        Task<IEnumerable<BudgetModel>> GetListBudgetModelAsync(Guid fixedAssetId);

    }
}
