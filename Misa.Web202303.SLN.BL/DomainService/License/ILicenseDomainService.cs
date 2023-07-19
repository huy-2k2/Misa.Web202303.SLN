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
        /// <summary>
        /// phương thức validate khi filter dữ liệu (kiểm tra xem dữ liệu để filter có hợp lệ không)
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="pageSize">kích thước page</param>
        /// <param name="currentPage">page hiện tại</param>
        void FilterInputValdiate(int pageSize, int currentPage);

        /// <summary>
        /// hàm valdiate khi thêm mới chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="licenseCreateDto">đối tượng LicenseCreateDto</param>
        /// <returns></returns>
        Task CreateValidateAsync(LicenseCreateDto licenseCreateDto);

        /// <summary>
        /// hàm validate khi xóa nhiều chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="listId">danh sách id chứng từ cần xóa</param>
        /// <returns></returns>
        Task DeleteListValidateAsync(IEnumerable<Guid> listId);

        /// <summary>
        /// hàm validate khi update chứng từ
        /// created by: NQ Huy(08/07/2023)
        /// </summary>
        /// <param name="licenseId">id chứng từ</param>
        /// <param name="licenseUpdateDto">đối tượng LicenseCreateDto</param>
        /// <returns></returns>
        Task UpdateValidateAsync(Guid licenseId, LicenseUpdateDto licenseUpdateDto);

    }
}
