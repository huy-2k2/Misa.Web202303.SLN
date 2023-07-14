using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseDetailEntity = Misa.Web202303.QLTS.DL.Entity.LicenseDetail;

namespace Misa.Web202303.QLTS.DL.Repository.LicenseDetail
{
    public interface ILicenseDetailRepository: IBaseRepository<LicenseDetailEntity>
    {

        Task<IEnumerable<LicenseDetailEntity>> GetListFAExistedAsync(string listFAId);

        Task<IEnumerable<LicenseDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId);
    }
}
