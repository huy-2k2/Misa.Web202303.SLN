using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.AutoMapper
{
    /// <summary>
    /// lớp định nghĩa các entity  FixedAssetCategory chuyển đổi qua nhau, sử dụng cho automapper
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryProfile : Profile
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        public FixedAssetCategoryProfile()
        {
            CreateMap<FixedAssetCategory, FixedAssetCategoryDto>();
            CreateMap<FixedAssetCategoryUpdateDto, FixedAssetCategory>();
            CreateMap<FixedAssetCategoryCreateDto, FixedAssetCategory>();
            CreateMap<FixedAssetCategoryImportDto, FixedAssetCategory>();
        }
    }
}
