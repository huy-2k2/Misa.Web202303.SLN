using Misa.Web202303.SLN.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.ImportService
{
    public interface IBaseImportService<TEntity>
    {
        /// <summary>
        /// hàm validate file excel trước khi impport vào database
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<ImportErrorEntity<TEntity>> ValidateAsync(MemoryStream stream);

    }
}
