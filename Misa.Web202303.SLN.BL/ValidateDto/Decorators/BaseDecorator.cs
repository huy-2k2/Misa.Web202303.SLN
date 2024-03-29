﻿using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ValidateDto.Decorators
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
        /// attribute tương ứng
        /// </summary>
        public BaseAttribute? attribute;

        /// <summary>
        /// tên trường dữ liệu
        /// </summary>
        public string? Name;

        /// <summary>
        /// giá trị dữ liệu
        /// </summary>
        public dynamic? propValue;

        /// <summary>
        /// thực hiện hàm handle và gọi hàm validate của decorator tiếp theo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        public void Validate()
        {
            Handle();
            nextDecorator?.Validate();
        }

        /// <summary>
        /// logic validate implement bởi các decorator kế thừa
        /// </summary>
        protected abstract void Handle();
    }

}
