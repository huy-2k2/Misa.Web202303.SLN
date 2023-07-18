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
        /// <summary>
        /// lấy ra danh sách budget_detail có budget_id và fixed_asset_id nối lại thì nằm trong 1 list id cho trước
        /// </summary>
        /// <param name="listBFId">danh sách id của fixed_asset và budget nối cách nhau bởi dấu ".", giữa nhiều cặp id cách nhau bởi dấu "," </param>
        /// <returns>Danh sách BudgetDetailEntity</returns>
        Task<IEnumerable<BudgetDetailEntity>> GetListExistedBFAsync(string listBFId);

        /// <summary>
        /// xóa các budget_detail theo danh sách license_id
        /// </summary>
        /// <param name="listLicenseId">danh sách license_id, các id nối cách nhau bởi dấu ","</param>
        /// <returns></returns>
        Task DeleteListByListLicenseId(string listLicenseId);

        /// <summary>
        /// xóa các budget_detail theo danh sách license_detail_id
        /// </summary>
        /// <param name="listLicenseDetailId">danh sách license_detail_id, các id cách nhau bởi dấu ","</param>
        /// <returns></returns>
        Task DeleteByListLicenseDetailId(string listLicenseDetailId);

        /// <summary>
        /// lấy ra các budget_detail có id nằm trong list_id cho trước, và thuộc một chứng từ có id cho trước
        /// </summary>
        /// <param name="licenseId">id của chứng từ</param>
        /// <param name="listId">danh sách budget_detail_id nối cách nhau bởi dấu ","</param>
        /// <returns>danh sách BudgetDetailEntity thỏa mã điều kiện</returns>
        Task<IEnumerable<BudgetDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId);
    }
}
