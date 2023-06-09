using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
{
    /// <summary>
    /// khời tạo decorator
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class InitDecorator : BaseDecorator
    {
        /// <summary>
        /// hàm khởi tạo nên không validate 
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        protected override ValidateError? Handle()
        {
            return null;
        }
    }
}
