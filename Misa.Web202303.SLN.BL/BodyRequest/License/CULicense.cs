using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.BodyRequest.License
{
    public class CULicense
    {
        /// <summary>
        /// thông tin của chứng từ
        /// </summary>
        public LicenseCUDto license { get; set; }

        /// <summary>
        /// danh sách tài sản (trong tài sản thì có ngân sách)
        /// </summary>
        public IEnumerable<LicenseFixedAssets> list_fixed_asset { get; set; }

        /// <summary>
        /// danh sách chi tiết chứng từ xóa
        /// </summary>
        public IEnumerable<Guid> list_fixed_asset_id_delete { get; set; }

        /// <summary>
        /// danh sách chi tiết nguồn hình thành xóa
        /// </summary>
        public IEnumerable<Guid> list_budget_detail_id_delete { get; set; }
    }
}
