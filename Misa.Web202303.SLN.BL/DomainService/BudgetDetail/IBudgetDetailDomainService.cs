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
        Task CreateListValidateAsync(IEnumerable<BudgetDetailCreateDto> listBudgetDetailCreateDto);

        Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId);

        Task UpdateListValidateAsync(IEnumerable<BudgetDetailUpdateDto> listBudgetDetailUpdateDto);
    }
}
