using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service;

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
    }
}
