using Misa.Web202303.SLN.BL.Service.Dto;
using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.SLN.DL.Entity.FixedAsset;

namespace Misa.Web202303.SLN.BL.ImportService.FixedAsset
{
    public interface IFixedAssetImportService : IBaseImportService<FixedAssetEntity>
    {
    }
}
