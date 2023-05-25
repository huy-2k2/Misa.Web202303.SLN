using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
{
    public abstract class BaseDecorator
    {
        public BaseDecorator? nextDecorator;
        public BaseAttribute? attribute;
        public dynamic? propValue;
        protected abstract void Handle();

        public void Validate()
        {
            Handle();
            nextDecorator?.Validate();
        }
    }

}
