using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseAttribute : Attribute
    {
        public string Name { get; set; }
        
        public BaseAttribute(string name) {
            Name= name;
        }
    }
}
