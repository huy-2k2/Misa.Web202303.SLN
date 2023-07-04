using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Decorators
{
    /// <summary>
    /// validate năm trùng năm hiện tại
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class CurrentYearDecorator : BaseDecorator
    {
        /// <summary>
        /// logic valdiate
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        protected override ValidateError? Handle()
        {
          
            var value = (int)PropValue;
            var currentYear = DateTime.Now.Year;
            if(value != currentYear)
            {
                return new ValidateError()
                {
                    FieldNameError = this.FieldNameError,
                    Message = string.Format(ErrorMessage.EqualError, Name, DateTime.Now.Year)

                };
            } else
                return null;
        }
    }
}
