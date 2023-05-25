using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Misa.Web202303.SLN.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến tài sản
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetController : BaseController<FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>
    {
        /// <summary>
        /// Sử dụng dịch vụ của IFixedASsetService
        /// </summary>
        private readonly IFixedAssetService _fixedAssetService;

        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetService"></param>
        public FixedAssetController(IFixedAssetService fixedAssetService):base(fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }

        /// <summary>
        /// phương thức sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId"></param>
        /// <param name="fixedAssetUpdateDto"></param>
        /// <returns></returns>

        // PUT api/<ValuesController>/5
        [HttpPut("{fixedAssetId}")]
        public  async Task<FixedAssetDto> Put([FromRoute]Guid fixedAssetId, [FromBody] FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            return await _fixedAssetService.UpdateAsync(fixedAssetId, fixedAssetUpdateDto);
        }

        /// <summary>
        /// phương thức thêm mới và nhân bản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedCreateAsset"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FixedAssetDto> Post([FromBody] FixedAssetCreateDto fixedCreateAsset)
        {
            return await _fixedAssetService.InsertAsync(fixedCreateAsset);
        }

        /// <summary>
        /// phướng thức lấy mã tài sản gợi ý
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        [HttpGet("recommendFixedAssetCode")]
        public async Task<string> GetRecommendFixedAssetCode()
        {
            var result = await _fixedAssetService.GetRecommendFixedAssetCodeAsync();
            return result;
        }

        /// <summary>
        /// phương thức kiểm tra mã tài sản có tồn tại không khi thêm vào sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCode"></param>
        /// <param name="fixedAssetId"></param>
        /// <returns></returns>
        [HttpGet("checkFixedAssetCodeExisted")]
        public async Task<bool> CheckAssetCodeExisted([FromQuery] string fixedAssetCode, [FromQuery] Guid? fixedAssetId)
        {
            var result = await _fixedAssetService.CheckAssetCodeExisted(fixedAssetCode, fixedAssetId);
            return result;
        }

        /// <summary>
        /// phương thức xóa nhiều tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<bool> DeleteAsync([FromBody] IEnumerable<Guid> listFixedAssetId)
        {
            var result = await _fixedAssetService.DeleteAsync(listFixedAssetId);
            return result;
        }

        /// <summary>
        /// phương thức filter và phân trang tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            return await _fixedAssetService.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
        }

    }
}
