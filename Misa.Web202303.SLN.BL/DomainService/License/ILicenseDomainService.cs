using Misa.Web202303.QLTS.BL.Service.License;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.License
{
    public interface ILicenseDomainService
    {
        void FilterInputValdiate(int pageSize, int currentPage);
        Task CreateValidateAsync(LicenseCreateDto licenseCreateDto);

        Task DeleteListValidateAsync(IEnumerable<Guid> listId);

        Task UpdateValidateAsync(Guid licenseId, LicenseUpdateDto licenseUpdateDto);

    }
}
