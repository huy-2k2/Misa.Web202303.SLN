using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Decorators
{
    /// <summary>
    /// khời tạo decorator
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class InitDecorator : BaseDecorator
    {
        /// <summary>
        /// hàm thực hiện logic validate
        /// created by: NQ Huy(07/05/2023)
        /// </summary>
        /// <returns>trả về lỗi nếu validate sai</returns>
        protected override ValidateError? Handle()
        {
            return null;
        }
    }
}
