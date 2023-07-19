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
        /// <summary>
        /// hàm validate create khi thêm license detail
        /// </summary>
        /// <param name="listCreateDto">listCreateDto</param>
        /// <returns></returns>
        Task CreateListValidateAsync(IEnumerable<LicenseDetailCreateDto> listCreateDto);

        /// <summary>
        /// hàm validate khi xóa nhiều license detail của 1 chứng từ
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="licenseId">id của license tương ứng</param>
        /// <param name="listDetailId">danh sách list license detail id</param>
        /// <returns></returns>
        Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId);
    }
}
