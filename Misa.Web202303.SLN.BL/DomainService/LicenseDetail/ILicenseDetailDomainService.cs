using Misa.Web202303.QLTS.BL.Service.LicenseDetail;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.LicenseDetail
{
    public interface ILicenseDetailDomainService
    {
        Task CreateListValidateAsync(IEnumerable<LicenseDetailCreateDto> listCreateDto);

        Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId);
    }
}
