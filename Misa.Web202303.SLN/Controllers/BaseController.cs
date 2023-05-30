using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service;
using Misa.Web202303.SLN.BL.Service.FixedAsset;

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
        public virtual async Task<TEntityDto> GetAsync(Guid id)
        {
            var entityDto = await _baseService.GetAsync(id);
            return entityDto;
        }

        /// <summary>
        /// phương thức lấy rất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IEnumerable<TEntityDto>> GetAsync()
        {
            return await _baseService.GetAsync();
        }

        /// <summary>
        /// sửa 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<TEntityDto> PutAsync([FromRoute] Guid id, [FromBody] TEntityUpdateDto entityUpdateDto) 
        {
            return await _baseService.UpdateAsync(id, entityUpdateDto);
        }

        /// <summary>
        /// thêm 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<TEntityDto> Post([FromBody] TEntityCreateDto entityCreateDto)
        {
            return await _baseService.InsertAsync(entityCreateDto);
        }

        /// <summary>
        /// phương thức xóa 1 bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            return await _baseService.DeleteAsync(id);
        }

            
        [HttpGet("isCodeExisted")]
        public async Task<bool> CheckAssetCodeExisted([FromQuery] string fixedAssetCode, [FromQuery] Guid? fixedAssetId)
        {
            var result = await _baseService.CheckCodeExisted(fixedAssetCode, fixedAssetId);
            return result;
        }

    }
}
