using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Attributes
{
    public class Required : BaseAttribute
    {
        public Required(string name) : base(name)
        {
        }
    }
}
