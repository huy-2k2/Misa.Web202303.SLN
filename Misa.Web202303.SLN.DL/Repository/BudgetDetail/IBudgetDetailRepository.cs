using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;


namespace Misa.Web202303.QLTS.DL.Repository.BudgetDetail
{
    public interface IBudgetDetailRepository : IBaseRepository<BudgetDetailEntity>
    {
        Task<IEnumerable<BudgetDetailEntity>> GetListExistedBFAsync(string listBFId);

        Task DeleteListByListLicenseId(string listLicenseId);

        Task DeleteByListLicenseDetailId(string listLicenseDetailId);

        Task<IEnumerable<BudgetDetailEntity>> GetListExistedOfLicenseAsync(Guid LicenseId, string listId);
    }
}
