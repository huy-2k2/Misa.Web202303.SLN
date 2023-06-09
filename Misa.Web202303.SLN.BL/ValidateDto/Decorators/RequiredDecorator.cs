using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
{
    /// <summary>
    /// kiểm tra bắt buộc nhập
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class RequiredDecorator : BaseDecorator
    {

        /// <summary>
        /// hàm thực hiện logic validate
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        protected override ValidateError? Handle()
        {
            if (propValue == null || string.IsNullOrEmpty(propValue))
            {
                return new ValidateError()
                {
                   FieldNameError = this.FieldNameError,
                   Message = string.Format(ErrorMessage.RequiredError, Name),
                };
            }
            else return null;
        }
    }
}
