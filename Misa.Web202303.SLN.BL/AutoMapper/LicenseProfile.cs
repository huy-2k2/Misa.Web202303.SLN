using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.BL.Service.License;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;

namespace Misa.Web202303.QLTS.BL.AutoMapper
{
    /// <summary>
    /// lớp định nghĩa các entity License chuyển đổi qua nhau, sử dụng cho automapper
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class LicenseProfile : Profile
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(27/06/2023)
        /// </summary>
        public LicenseProfile()
        {
            CreateMap<LicenseEntity, LicenseDto>();
            CreateMap<LicenseCreateDto, LicenseEntity>();
            CreateMap<LicenseUpdateDto, LicenseEntity>();
        }
    }
}
