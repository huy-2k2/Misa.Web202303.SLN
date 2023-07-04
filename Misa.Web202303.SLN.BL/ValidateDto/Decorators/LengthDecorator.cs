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
        protected override ValidateError? Handle()
        {
            string value = Convert.ToString(PropValue);
            var lengthAttribute = (Length)Attr;
            if (value.Length > lengthAttribute.Max || value.Length < lengthAttribute.Min)
            {
                return new ValidateError()
                {
                   FieldNameError = this.FieldNameError,
                   Message = string.Format(ErrorMessage.LengthError, Name, lengthAttribute.Min, lengthAttribute.Max)
                };
            }
            else
                return null;
        }
    }
}
