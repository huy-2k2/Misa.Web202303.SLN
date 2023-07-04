using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.BL.ImportService.Department
{
    /// <summary>
    /// định nghĩa các phương thức riêng, giao tiếp với bên ngoài của departmentImportService
    /// created by: nqhuy(10/06/2023)
    ///<summary>
    public interface IDepartmentImportService:IBaseImportService<DepartmentEntity>
    {
    }
}
