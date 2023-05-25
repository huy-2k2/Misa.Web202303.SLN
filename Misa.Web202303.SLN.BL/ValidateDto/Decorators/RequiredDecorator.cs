using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
{
    public class RequiredDecorator : BaseDecorator
    {

        protected override void Handle()
        {
            if (propValue == null || string.IsNullOrEmpty(propValue)) {
                throw new Exception($"yêu cầu nhập {attribute.Name}");
            }
        }
    }
}
