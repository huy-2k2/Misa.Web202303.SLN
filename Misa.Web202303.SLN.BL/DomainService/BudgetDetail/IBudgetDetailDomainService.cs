using Misa.Web202303.QLTS.BL.Service.BudgetDetail;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.BudgetDetail
{
    public interface IBudgetDetailDomainService
    {
        /// <summary>
        /// hàm validate khi thêm nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listBudgetDetailCreateDto">list các đối tượng BudgetDetailCreateDto</param>
        /// <returns></returns>
        Task CreateListValidateAsync(IEnumerable<BudgetDetailCreateDto> listBudgetDetailCreateDto);

        /// <summary>
        /// hàm validate khi delete nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="licenseId">license id tương ứng của list budget detail</param>
        /// <param name="listDetailId">list budget detail id</param>
        /// <returns></returns>
        Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId);

        /// <summary>
        /// validate update nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listBudgetDetailUpdateDto">danh sách đối tượng BudgetDetailUpdateDto</param>
        /// <returns></returns>
        Task UpdateListValidateAsync(IEnumerable<BudgetDetailUpdateDto> listBudgetDetailUpdateDto);
    }
}
