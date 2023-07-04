using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.RecommendCode
{
    public class RecommendCodeService : IRecommendCodeService
    {

        /// <summary>
        /// phương thức tạo mã mới từ tiền tố và hậu tố của mã cũ
        /// </summary>
        /// <param name="prefix">tiền tố</param>
        /// <param name="postfix">hậu tố</param>
        /// <returns>mã gợi ý mới</returns>
        public string CreateRecommendCode(string prefix, string  postfix)
        {
            // tính toán hậu tố mới
            var newPostFix = "0";
            if (postfix.Length != 0)
                newPostFix = $"{long.Parse(postfix) + 1}";

            // cộng thêm các chữ số 0 vào hậu tố mới
            for (int i = 0; i < postfix.Length; i++)
            {
                if (postfix[i] != '0')
                    break;
                if (newPostFix.Length < postfix.Length)
                    newPostFix = '0' + newPostFix;
            }

            return prefix + newPostFix;
        }
    }
}
