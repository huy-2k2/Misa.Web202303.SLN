using AutoMapper;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.AutoMapper
{
    /// <summary>
    /// lớp định nghĩa các entity FixedAsset chuyển đổi qua nhau, sử dụng cho automapper
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetProfile : Profile
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        public FixedAssetProfile()
        {
            CreateMap<FixedAsset, FixedAssetDto>();
            CreateMap<FixedAssetCreateDto, FixedAsset>();
            CreateMap<FixedAssetUpdateDto, FixedAsset>();
        }

    }
}
