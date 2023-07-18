using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.BudgetDetail;
using Misa.Web202303.QLTS.BL.Service.LicenseDetail;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.AutoMapper
{
    public class LicenseDetailProfile : Profile
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(27/06/2023)
        /// </summary>
        public LicenseDetailProfile()
        {
            CreateMap<LicenseDetailCreateDto, LicenseDetail>();
        }
    }
}
