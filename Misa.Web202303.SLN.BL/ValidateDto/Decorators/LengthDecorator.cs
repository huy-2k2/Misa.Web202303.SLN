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
    /// kiểm tra độ dài chuỗi
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class LengthDecorator : BaseDecorator
    {
        /// <summary>
        /// logic validate
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        protected override void Handle()
        {
            string value = Convert.ToString(propValue);
            var lengthAttribute = (Length)attribute;
            if(value.Length > lengthAttribute.Max || value.Length < lengthAttribute.Min)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.LengthError, Name, lengthAttribute.Min, lengthAttribute.Max),
                    DevMessage = string.Format(ErrorMessage.LengthError, Name, lengthAttribute.Min, lengthAttribute.Max),
                    ErrorCode = ErrorCode.DataValidate
                };
            }
        }
    }
}
