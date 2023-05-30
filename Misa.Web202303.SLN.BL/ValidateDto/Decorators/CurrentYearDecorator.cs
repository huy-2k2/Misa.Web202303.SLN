using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
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
        protected override void Handle()
        {
          
            var value = (int)propValue;
            var currentYear = DateTime.Now.Year;
            if(value != currentYear)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.EqualError, Name, currentYear),
                    DevMessage = string.Format(ErrorMessage.EqualError, Name, currentYear),
                    ErrorCode = ErrorCode.DataValidate
                };
            }
        }
    }
}
