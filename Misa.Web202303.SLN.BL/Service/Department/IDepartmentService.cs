using Misa.Web202303.SLN.BL.ImportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của DepartmentService, ngoài các phương thức của IBaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public interface IDepartmentService : IBaseService<DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>
    {
        /// <summary>
        /// import dữ liệu tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        public Task<ImportErrorEntity<DepartmentEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit);
    }
}
