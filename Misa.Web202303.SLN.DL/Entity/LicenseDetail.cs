using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class LicenseDetail : BaseEntity
    {
        /// <summary>
        /// license detail id
        /// </summary>
        public Guid license_detail_id { set; get; }

        /// <summary>
        /// id chứng từ
        /// </summary>
        public Guid license_id { set; get; }

        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid fixed_asset_id { set; get; }

        /// <summary>
        /// số thứ tự tài sản trong chứng từ
        /// </summary>
        public int license_detail_index { set; get; }

    }
}
