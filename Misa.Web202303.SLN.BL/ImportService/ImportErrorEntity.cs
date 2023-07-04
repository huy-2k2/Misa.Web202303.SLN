using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ImportService
{
    /// <summary>
    /// lớp để khởi tạo dữ liệu trả về khi validate dữ liệu trong file lúc import
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ImportErrorEntity<TEntity>
    {
        /// <summary>
        /// lỗi của table
        /// </summary>
        public IEnumerable<IEnumerable<ValidateError>> ErrorOfTable { get; set; }

        /// <summary>
        /// dữ liệu dùng để hiển thị khi có lỗi xảy ra
        /// </summary>
        public IEnumerable<IEnumerable<string>> RawEntities { get; set; }

        /// <summary>
        /// dữ liệu import của table, column
        /// </summary>
        public IEnumerable<ImportEntity> ListImportData { get; set; }

        /// <summary>
        /// biểu thị có validate thành công hay không
        /// </summary>
        public bool IsPassed { get; set; } 
        
        /// <summary>
        /// danh sách trả về khi không có lỗi
        /// </summary>
        public IEnumerable<TEntity>? ListEntity { get; set; }

        /// <summary>
        /// dữ liệu của tiêu đề
        /// </summary>
        public IEnumerable<string> THead { get; set; }

    }
}
