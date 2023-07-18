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
    /// base decorator
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public abstract class BaseDecorator
    {
        /// <summary>
        /// decorator tiếp theo
        /// </summary>
        public BaseDecorator? nextDecorator;

        /// <summary>
        /// Attr tương ứng
        /// </summary>
        public BaseAttribute? Attr;

        /// <summary>
        /// tên trường dữ liệu
        /// </summary>
        public string? Name;

        /// <summary>
        /// giá trị dữ liệu
        /// </summary>
        public dynamic? PropValue;

        /// <summary>
        /// tên trường bị lỗi
        /// </summary>
        public string FieldNameError;

        /// <summary>
        /// chi tiết lỗi
        /// </summary>
        public ValidateError Error;

        /// <summary>
        /// thực hiện hàm handle và gọi hàm validate của decorator tiếp theo
        /// created by: nqhuy(21/05/2023)
        /// <returns></returns>
        /// </summary>
        public void Validate()
        {
            if (nextDecorator != null)
            {
                nextDecorator.Validate();
                if (nextDecorator.Error != null)
                {
                    this.Error = nextDecorator.Error;
                }
                else
                    Error = Handle();


            }
        }

        /// <summary>
        /// hàm thực hiện logic validate
        /// created by: NQ Huy(07/05/2023)
        /// </summary>
        /// <returns>trả về lỗi nếu validate sai</returns>
        protected abstract ValidateError? Handle();
    }

}
