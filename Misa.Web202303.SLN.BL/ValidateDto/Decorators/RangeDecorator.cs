﻿using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RangeAttribute = Misa.Web202303.SLN.BL.ValidateDto.Attributes.Range;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
{
    /// <summary>
    /// kiểm tra giá trị nhằm trong khoảng
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class RangeDecorator : BaseDecorator
    {
        /// <summary>
        /// logic validate
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        protected override ValidateError? Handle()
        {
            var value = (double)propValue;
            var rangeAttribute = (RangeAttribute)attribute;
            if (value < rangeAttribute.Min || value > rangeAttribute.Max)
            {
                return new ValidateError()
                {
                    FieldNameError = this.FieldNameError,
                    Message = string.Format(ErrorMessage.RangeError, Name, rangeAttribute.Min, rangeAttribute.Max)
                };
            }
            else
                return null;
        }
    }
}
