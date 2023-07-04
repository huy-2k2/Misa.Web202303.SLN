using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEntity = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;

namespace Misa.Web202303.QLTS.BL.ImportService.FixedAssetCategory
{
    /// <summary>
    /// định nghĩa các phương thức riêng, giao tiếp với bên ngoài của fixedAssetCategoryImportService
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public interface IFixedAssetCategoryImportService : IBaseImportService<FixedAssetCategoryEntity>
    {
    }
}
