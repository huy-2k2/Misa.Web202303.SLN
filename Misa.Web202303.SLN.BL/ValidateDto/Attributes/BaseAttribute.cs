using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Attributes
{
    /// <summary>
    /// base Attr
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseAttribute : Attribute
    {
        
    }
}
