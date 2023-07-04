using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.Budget;
using Misa.Web202303.QLTS.BL.Service.Dto;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.AutoMapper
{
    /// <summary>
    /// lớp định nghĩa các entity Budget chuyển đổi qua nhau, sử dụng cho automapper
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class BudgetProfile : Profile
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(27/06/2023)
        /// </summary>
        public BudgetProfile()
        {
            CreateMap<Budget, BudgetDto>();
            CreateMap<BudgetCreateDto, Budget>();
            CreateMap<BudgetUpdateDto, Budget>();
        }
    }
}
