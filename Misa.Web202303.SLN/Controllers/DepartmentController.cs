using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service;
using Misa.Web202303.SLN.BL.Service.Department;

namespace Misa.Web202303.SLN.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến phòng ban
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentController : BaseController<DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>
    {
        /// <summary>
        /// sử dụng dịch vụ của IDepartmentService
        /// </summary>
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentService"></param>
        public DepartmentController(IDepartmentService departmentService) : base(departmentService)
        {
            _departmentService = departmentService;
        }

    }
}
