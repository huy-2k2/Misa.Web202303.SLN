using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.BL.Service;
using Misa.Web202303.QLTS.BL.Service.Department;
using System.Net;

namespace Misa.Web202303.QLTS.API.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến phòng ban
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentController : BaseController<DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>
    {
        #region
        /// <summary>
        /// sử dụng dịch vụ của IDepartmentService
        /// </summary>
        private readonly IDepartmentService _departmentService;
        #endregion


        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentService"></param>
        public DepartmentController(IDepartmentService departmentService) : base(departmentService)
        {
            _departmentService = departmentService;
        }
        #endregion

        #region
        /// <summary>
        /// import file excel vào db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="file">file từ frontend gửi</param>
        /// <param name="isSubmit">biến kiểm tra là submit hay validate file</param>
        /// <returns>dữ liệu trong file, dữ liệu validate</returns>
        [HttpPost("file")]
        public async Task<IActionResult> ImportFileAsync([FromForm] IFormFile file, [FromQuery] bool isSubmit)
        {

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var result = await _departmentService.ImportFileAsync(stream, isSubmit);
                return StatusCode((int)HttpStatusCode.Created, result);
            }
        }
        #endregion
    }
}
