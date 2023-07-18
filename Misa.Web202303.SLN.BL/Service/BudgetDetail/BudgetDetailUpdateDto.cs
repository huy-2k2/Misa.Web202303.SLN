using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service.BudgetDetail
{
    public class BudgetDetailUpdateDto
    {
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid budget_detail_id { get; set; }

        /// <summary>
        /// khóa ngoại, budget id
        /// </summary>
        public Guid budget_id { get; set; }

        /// <summary>
        /// khóa ngoại, fixed asset id
        /// </summary>
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// giá trị ngân sách
        /// </summary>
        [Higher(0), NameAttribute(FieldName.BudgetValue)]
        public double budget_value { get; set; }    
    }
}
