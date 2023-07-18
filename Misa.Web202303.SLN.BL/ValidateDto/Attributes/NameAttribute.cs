using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto.Attributes
{
    /// <summary>
    /// lấy ra tên của trường dữ liệu
    /// </summary>
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="name">tên của property</param>
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
