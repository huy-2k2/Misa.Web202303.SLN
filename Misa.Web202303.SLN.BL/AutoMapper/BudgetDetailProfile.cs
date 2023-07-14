using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.Budget;
using Misa.Web202303.QLTS.BL.Service.BudgetDetail;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BudgetInLicenseRequestBody = Misa.Web202303.QLTS.BL.BodyRequest.License.Budget;

namespace Misa.Web202303.QLTS.BL.AutoMapper
{
    public class BudgetDetailProfile : Profile
    {
        public BudgetDetailProfile()
        {
            CreateMap<BudgetDetailCreateDto, BudgetDetail>();
            CreateMap<BudgetDetailUpdateDto, BudgetDetail>();
            CreateMap<BudgetDetail, BudgetDetailDto>();
            CreateMap<BudgetInLicenseRequestBody, BudgetDetailCreateDto>();
            CreateMap<BudgetInLicenseRequestBody, BudgetDetailUpdateDto>();
        }
    }
}
