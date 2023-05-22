using Microsoft.Extensions.Configuration;
using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEnitty = Misa.Web202303.SLN.DL.Entity.FixedAssetCategory;

namespace Misa.Web202303.SLN.DL.Repository.FixedAssetCategory
{
    /// <summary>
    /// thực thi các phương thức của IFixedAssetCategoryRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FixedAssetCategoryRepository : BaseRepository<FixedAssetCategoryEnitty>, IFixedAssetCategoryRepository
    {
        /// <summary>
        /// hàm khởi tạo
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration"></param>
        public FixedAssetCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// lấy ra tên của table trong csdl ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override string GetTableName()
        {
            return "fixed_asset_category";
        }
    }
}
