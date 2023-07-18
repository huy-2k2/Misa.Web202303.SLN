using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetModel = Misa.Web202303.QLTS.DL.Model.Budget;


namespace Misa.Web202303.QLTS.BL.Service.Budget
{
    public interface IBudgetService : IBaseService<BudgetDto, BudgetUpdateDto, BudgetCreateDto>
    {
        /// <summary>
        /// lấy danh sách budget Model của 1 tài sản
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách budget Model của 1 tài sản</returns>
        Task<IEnumerable<BudgetModel>> GetListBudgetModelAsync(Guid fixedAssetId);
    }
}
