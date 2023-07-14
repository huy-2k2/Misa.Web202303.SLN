using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.BL.Service;
using Misa.Web202303.QLTS.BL.Service.Budget;

namespace Misa.Web202303.QLTS.API.Controllers
{
    public class BudgetController : BaseController<BudgetDto, BudgetUpdateDto, BudgetCreateDto>
    {
        private readonly IBudgetService _budgetService;

        /// <summary>
        /// hàm khơi tạo
        /// created by : NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="budgetService">budgetService</param>
        public BudgetController(IBudgetService budgetService) : base(budgetService)
        {
            _budgetService= budgetService;
        }

        /// <summary>
        /// lấy danh sách budget Model của 1 tài sản
        /// created by : NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <returns>danh sách budget Model của 1 tải sản</returns>
        [HttpGet("listModel")]
        public async Task<IActionResult> GetListModelAsync([FromQuery] Guid fixedAssetId)
        {
            var result = await _budgetService.GetListBudgetModelAsync(fixedAssetId);
            return Ok(result);
        }
    }
}
