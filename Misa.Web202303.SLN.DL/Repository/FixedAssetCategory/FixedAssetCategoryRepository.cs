using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEnitty = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;

namespace Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory
{
    /// <summary>
    /// thực thi các phương thức của IFixedAssetCategoryRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FixedAssetCategoryRepository : BaseRepository<FixedAssetCategoryEnitty>, IFixedAssetCategoryRepository
    {
        #region
        /// <summary>
        /// hàm khởi tạo
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public FixedAssetCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region
        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>tên của table fixed asset category</returns>
        public override string GetTableName()
        {
            return "fixed_asset_category";
        }
        #endregion
    }
}
