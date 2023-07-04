using Misa.Web202303.QLTS.BL.Service.Dto;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.ImportService.FixedAsset
{
    /// <summary>
    /// định nghĩa các phương thức riêng, giao tiếp với bên ngoài của fixedAssetImportService
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public interface IFixedAssetImportService : IBaseImportService<FixedAssetEntity>
    {
    }
}
