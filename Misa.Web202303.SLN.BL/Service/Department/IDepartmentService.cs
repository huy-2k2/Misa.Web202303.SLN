using Misa.Web202303.QLTS.BL.ImportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.BL.Service.Department
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
        /// <param name="stream">file import dưới dạng stream</param>
        /// <param name="isSubmit">biến kiểm tra người dùng có đang submit</param>
        /// <returns>dữ liệu về file excel và dữ liệu valdiate</returns>
        public Task<ImportErrorEntity<DepartmentEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit);
    }
}
