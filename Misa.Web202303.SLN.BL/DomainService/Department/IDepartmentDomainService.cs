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
        /// <summary>
        /// validate khi thêm mới 
        /// created by: NQ Huy (05/06/2023)
        /// </summary>
        /// <param name="departmentCreateDto">đối tượng DepartmentCreateDto</param>
        /// <returns></returns>
        Task CreateValidateAsync(DepartmentCreateDto departmentCreateDto);


        /// <summary>
        /// validate khi update
        /// created by: NQ Huy (05/06/2023)
        /// </summary>
        /// <param name="deparmtentId">id của đối tượng update</param>
        /// <param name="departmentUpdateDto">đối tượng DepartmentUpdateDto</param>
        /// <returns></returns>
        Task UpdateValidateAsync(Guid deparmtentId, DepartmentUpdateDto departmentUpdateDto);
    }
}
