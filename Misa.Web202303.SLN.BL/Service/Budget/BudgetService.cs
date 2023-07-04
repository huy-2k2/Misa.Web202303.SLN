using AutoMapper;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetModel = Misa.Web202303.QLTS.DL.model.Budget;

using BudgetEntity = Misa.Web202303.QLTS.DL.Entity.Budget;

namespace Misa.Web202303.QLTS.BL.Service.Budget
{
    public class BudgetService : BaseService<BudgetEntity, BudgetDto, BudgetUpdateDto, BudgetCreateDto>, IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        
        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="budgetRepository"></param>
        /// <param name="mapper"></param>
        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper) : base(budgetRepository, mapper)
        {
            _budgetRepository = budgetRepository;
        }


        /// <summary>
        /// lấy danh sách budget model của 1 tài sản
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách budget model của 1 tài sản</returns>
        public async Task<IEnumerable<BudgetModel>> GetListBudgetModelAsync(Guid fixedAssetId)
        {
            // validate here
            var result = await _budgetRepository.GetListBudgetModelAsync(fixedAssetId);
            return result;
        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tên tài nguyên</returns>
        protected override string GetAssetName()
        {
            return AssetName.Budget;
        }
    }
}
