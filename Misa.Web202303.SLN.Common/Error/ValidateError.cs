using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.Common.Error
{
    /// <summary>
    /// khởi tạo dữ liệu lỗi khi validate
    /// Created by: NQ Huy(10/05/2023)
    /// </summary>
    public class ValidateError
    {
        /// <summary>
        /// mô tả chi tiết lỗi
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// tên trường bị lỗi
        /// </summary>
        public string FieldNameError { set; get; }
    }
}
