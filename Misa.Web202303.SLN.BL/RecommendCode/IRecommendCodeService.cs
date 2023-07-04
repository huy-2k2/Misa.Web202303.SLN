using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.RecommendCode
{
    public interface IRecommendCodeService
    {
     
        /// <summary>
        /// phương thức tạo mã mới từ tiền tố và hậu tố của mã cũ
        /// </summary>
        /// <param name="prefix">tiền tố</param>
        /// <param name="postfix">hậu tố</param>
        /// <returns>mã gợi ý mới</returns>
        string CreateRecommendCode(string prefix, string postfix);
    }
}
