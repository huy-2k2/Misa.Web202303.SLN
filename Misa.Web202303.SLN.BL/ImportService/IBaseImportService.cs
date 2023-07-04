using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ImportService
{
    /// <summary>
    /// định nghĩa các phương thức dùng để giao tiếp với bên ngoài của import service
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    /// <typeparam name="TEntity">entity tầng DL tương ứng với table cần import</typeparam>
    public interface IBaseImportService<TEntity>
    {
        /// <summary>
        /// hàm validate file excel trước khi impport vào database
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream">dữ liệu file ở dạng stream</param>
        /// <returns>dữ liệu của file, dữ liệu validate</returns>
        public Task<ImportErrorEntity<TEntity>> ValidateAsync(MemoryStream stream);

    }
}
