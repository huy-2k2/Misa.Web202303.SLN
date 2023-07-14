using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.API.CustomFilter;
using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.Common.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Misa.Web202303.QLTS.API.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến tài sản
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetController : BaseController<FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>
    {
        #region
        /// <summary>
        /// Sử dụng dịch vụ của IFixedASsetService
        /// </summary>
        private readonly IFixedAssetService _fixedAssetService;
        #endregion


        #region
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetService"></param>
        public FixedAssetController(IFixedAssetService fixedAssetService) : base(fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }
        #endregion

        #region

        /// <summary>
        /// phướng thức lấy mã tài sản gợi ý
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>mã tài sản gợi ý</returns>
        [HttpGet("recommendFixedAssetCode")]
        public async Task<IActionResult> GetRecommendFixedAssetCode()
        {
            var result = await _fixedAssetService.GetRecommendFixedAssetCodeAsync();
            return Ok(result);
        }

        /// <summary>
        /// phương thức Filter và phân trang tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="departmentId">mã phòng ban</param>
        /// <param name="fixedAssetCategoryId">mã loại tài sản</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("Filter")]
        public async Task<IActionResult> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            var result =  await _fixedAssetService.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
            return Ok(result);
        }

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
                var result = await _fixedAssetService.ImportFileAsync(stream, isSubmit);
                return StatusCode((int)HttpStatusCode.Created ,result);
            }
        }

        /// <summary>
        /// endpoint Filter phân trang tài sản chưa có chứng từ
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="body">dữ liệu phân trang</param>
        /// <returns>danh sách tài sản</returns>
        [HttpPost("filterNoLicense")]
        public async Task<IActionResult> GetFilterNotHasLicenseAsync([FromBody] FilterFixedAssetNoLicense body)
        {
            var result = await _fixedAssetService.GetFilterNotHasLicenseAsync(body);
            return Ok(result);
        }

        [HttpGet("listByLicenseId")]
        public async Task<IActionResult> GetListFixedAssetByLicenseId([FromQuery] Guid licenseId)
        {
            var result = await _fixedAssetService.GetListByLicenseIdAsync(licenseId);
            return Ok(result);
        }

        #endregion
    }
}
