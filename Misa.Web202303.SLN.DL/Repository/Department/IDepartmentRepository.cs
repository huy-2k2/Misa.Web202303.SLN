using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.DL.Repository.Department
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của DepartmentRepository, ngoài các phương thức của IBaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public interface IDepartmentRepository : IBaseRepository<DepartmentEntity>
    {
    }
}
