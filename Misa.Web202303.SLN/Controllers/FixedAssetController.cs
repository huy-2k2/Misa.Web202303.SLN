using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.Common.Exceptions;
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
        /// phương thức xóa nhiều tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        [HttpDelete("multiple")]
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
        [HttpGet("filter")]
        public async Task<object> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            return await _fixedAssetService.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
        }

        /// <summary>
        /// xuất tài sản ra file excel
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <typeparam name="IActionResult"></typeparam>
        /// <returns></returns>
        [HttpGet("excel")]
        public async Task<IActionResult> GetExcelFileAsync()
        {
            var stream = await _fixedAssetService.GetFixedAssetsExcelAsync();
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "fixedAssetInfo.xlsx");
        } 

    }
}
