using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEnitty = Misa.Web202303.SLN.DL.Entity.FixedAssetCategory;


namespace Misa.Web202303.SLN.DL.Repository.FixedAssetCategory
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của FixedAssetCategoryRepository, ngoài các phương thức của IBaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public interface IFixedAssetCategoryRepository : IBaseRepository<FixedAssetCategoryEnitty>
    {
    }
}
