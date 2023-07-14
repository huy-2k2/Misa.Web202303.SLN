using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.BudgetDetail
{
    public class BudgetDetailCreateDto
    {
        public Guid fixed_asset_id { get; set; }

        public Guid budget_id { get; set; }

        [Higher(0), NameAttribute(FieldName.BudgetValue)]
        public double budget_value { get; set; }    
    }
}
