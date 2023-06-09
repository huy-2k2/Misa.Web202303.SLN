using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    public class ImportEntity
    {
        /// <summary>
        /// số thứ tự cột trong file excel
        /// </summary>
        public int Import_column_index { get; set; }

        /// <summary>
        /// tên field tương ứng trong entity
        /// </summary>
        public string Prop_name { get; set; }

        /// <summary>
        /// kiểu dữ liệu
        /// </summary>
        public int Data_type { get; set; }

        /// <summary>
        /// tên table import vào
        /// </summary>
        public string Import_file_table { get; set; }

        /// <summary>
        /// số cột trong bảng của file excel
        /// </summary>
        public int Number_column { get; set; }
    }
}
