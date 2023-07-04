using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.API.CustomFilter;
using Misa.Web202303.QLTS.BL.Service;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using System.Net;

namespace Misa.Web202303.QLTS.API.Controllers
{
    /// <summary>
    /// controllerbase định nghĩa các phương thức chung, các controller khác kế thừa từ controllerbase
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntityDto">entity để nhận dữ liệu</typeparam>
    /// <typeparam name="TEntityUpdateDto">entity để update dữ liệu</typeparam>
    /// <typeparam name="TEntityCreateDto">entity để cập nhật dữ liệu</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomAuthorizationFilter))]

    public abstract class BaseController<TEntityDto, TEntityUpdateDto, TEntityCreateDto> : ControllerBase
    {
        #region
        protected readonly IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> _baseService;
        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="baseService"></param>
       
        public BaseController(IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region
        /// <summary>
        /// phương thức lấy ra 1 bản ghi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên cần lấy</param>
        /// <returns>tài nguyên cần lấy</returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetAsync(Guid id)
        {
            var entityDto = await _baseService.GetAsync(id);
            return Ok(entityDto);
        }

        /// <summary>
        /// phương thức lấy rất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tất cả tài nguyên trong 1 bảng</returns>
        [HttpGet]
        public virtual async Task<IActionResult> GetAsync()
        {
            var result =  await _baseService.GetAsync();
            return Ok(result);
        }

        /// <summary>
        /// sửa 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên cần sửa</param>
        /// <param name="entityUpdateDto">dữ liệu tài nguyên cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] TEntityUpdateDto entityUpdateDto) 
        {
           await _baseService.UpdateAsync(id, entityUpdateDto);
            return Ok();
        }

        /// <summary>
        /// thêm 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto">dữ liệu tài nguyên cần thêm</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntityCreateDto entityCreateDto)
        {
            await _baseService.InsertAsync(entityCreateDto);
            return StatusCode((int)HttpStatusCode.Created);
        }


        /// <summary>
        /// check mã code có tồn tại hay không khi update hoạc insertv
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code">mã tài nguyên</param>
        /// <param name="id">id tài nguyên (là rỗng trong trường hợp thêm mới)</param>
        /// <returns>false nếu mã chưa tồn tại, true nếu mã tồn tại</returns>
        [HttpGet("isCodeExisted")]
        public async Task<IActionResult> CheckAssetCodeExisted([FromQuery] string code, [FromQuery] Guid? id)
        {
            var result = await _baseService.CheckCodeExisted(code, id);
            return Ok(result);
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listId">danh sách id tài nguyên cần xóa</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteListAsync(IEnumerable<Guid> listId) {
             await _baseService.DeleteListAsync(listId);
            return Ok();
            
        }
        #endregion
    }
}
