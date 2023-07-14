using Misa.Web202303.QLTS.BL.Service.Department;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.Department
{
    public interface IDepartmentDomainService
    {
        Task CreateValidateAsync(DepartmentCreateDto departmentCreateDto);

        Task UpdateValidateAsync(Guid deparmtentId, DepartmentUpdateDto departmentUpdateDto);
    }
}
