using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    /// <summary>
    /// lấy dữ liệu về import_column và import_file
    /// Created by: NQ Huy(10/05/2023)
    /// </summary>
    public class ImportEntity
    {
        /// <summary>
        /// số thứ tự cột trong file excel
        /// </summary>
        public int import_column_index { get; set; }

        /// <summary>
        /// tên field tương ứng trong entity
        /// </summary>
        public string prop_name { get; set; }

        /// <summary>
        /// kiểu dữ liệu
        /// </summary>
        public int data_type { get; set; }

        /// <summary>
        /// tên table import vào
        /// </summary>
        public string import_file_table { get; set; }

        /// <summary>
        /// số cột trong bảng của file excel
        /// </summary>
        public int number_column { get; set; }
    }
}
