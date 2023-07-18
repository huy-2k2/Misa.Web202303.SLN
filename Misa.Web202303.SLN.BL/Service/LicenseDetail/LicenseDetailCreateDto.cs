using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.LicenseDetail
{
    public class LicenseDetailCreateDto
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid license_id { get; set; }

        /// <summary>
        /// khóa ngoại, id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }
    }
}
