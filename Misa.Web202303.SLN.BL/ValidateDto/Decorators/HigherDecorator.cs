using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Decorators
{
    /// <summary>
    /// validate giá trị phải lớn hơn min
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class HigherDecorator : BaseDecorator
    {
        /// <summary>
        /// logic validate
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override ValidateError? Handle()
        {
            var value = (double)PropValue;
            var attr = (Higher)Attr;
            var min = attr.Min;
            if (value < min)
            {
                return new ValidateError()
                {
                    FieldNameError = this.FieldNameError,
                    Message = string.Format(ErrorMessage.HigherError, Name, min)

                };
            }
            else
                return null;
        }
    }
}
