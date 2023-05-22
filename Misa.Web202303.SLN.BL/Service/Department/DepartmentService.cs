using AutoMapper;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.BL.Service.Department
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của Department, gồm các phương thức của IDepartmentService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentService : BaseService<DepartmentEntity, DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>, IDepartmentService
    {
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentRepository"></param>
        /// <param name="mapper"></param>
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository, mapper)
        {
        }

    }
}
