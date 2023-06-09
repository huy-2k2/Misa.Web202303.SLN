using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using System.Net;

namespace Misa.Web202303.SLN.Controllers
{
    /// <summary>
    /// controllerbase định nghĩa các phương thức chung, các controller khác kế thừa từ controllerbase
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TEntityUpdateDto"></typeparam>
    /// <typeparam name="TEntityCreateDto"></typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntityDto, TEntityUpdateDto, TEntityCreateDto> : ControllerBase
    {
        protected readonly IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> _baseService;

        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="baseService"></param>
        public BaseController(IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> baseService)
        {
            _baseService = baseService;
        }


        /// <summary>
        /// phương thức lấy ra 1 bản ghi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
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
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntityCreateDto entityCreateDto)
        {
            await _baseService.InsertAsync(entityCreateDto);
            return StatusCode((int)HttpStatusCode.Created);
        }


        /// <summary>
        /// check mã code có tồn tại hay không khi update hoạc insert
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCode"></param>
        /// <param name="fixedAssetId"></param>
        /// <returns></returns>
        [HttpGet("isCodeExisted")]
        public async Task<IActionResult> CheckAssetCodeExisted([FromQuery] string fixedAssetCode, [FromQuery] Guid? fixedAssetId)
        {
            var result = await _baseService.CheckCodeExisted(fixedAssetCode, fixedAssetId);
            return Ok(result);
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteListAsync(IEnumerable<Guid> listId) {
             await _baseService.DeleteListAsync(listId);
            return Ok();
            
        }
    }
}
