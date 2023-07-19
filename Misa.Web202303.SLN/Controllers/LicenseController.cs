using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.BL.BodyRequest.License;
using Misa.Web202303.QLTS.BL.Service;
using Misa.Web202303.QLTS.BL.Service.License;

namespace Misa.Web202303.QLTS.API.Controllers
{
    public class LicenseController : BaseController<LicenseDto, LicenseUpdateDto, LicenseCreateDto>
    {
        /// <summary>
        /// dùng để gọi service license
        /// </summary>
        private readonly ILicenseService _licenseService;

        /// <summary>
        /// hàm khởi tạo
        /// created by : NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="licenseService">licenseService</param>
        public LicenseController(ILicenseService licenseService) : base(licenseService)
        {
            _licenseService = licenseService;
        }

        /// <summary>
        /// created by : NQ Huy(27/06/2023)
        /// end point Filter, phân trang chứng từ)
        /// </summary>
        /// <param name="pageSize">kích thước page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns></returns>
        
        [HttpGet("Filter")]
        public async Task<IActionResult> GetLicenseModelAync([FromQuery] int pageSize, [FromQuery] int currentPage, [FromQuery] string? textSearch)
        {
            var result = await _licenseService.GetListLicenseModelAsync(pageSize, currentPage, textSearch);
            return Ok(result);
        }

        /// <summary>
        /// hàm lấy mã gợi ý cho chứng từ
        /// created by : NQ Huy(27/06/2023)
        /// </summary>
        /// <returns>mã gợi ý</returns>
        [HttpGet("recommendLicenseCode")]
        public async Task<IActionResult> GetRecommendCodeAsync()
        {
            var result = await _licenseService.GetRecommendAsync();
            return Ok(result);
        }

        [HttpPost("model")]
        public async Task<IActionResult> InsertModelAsync(CULicense cuLicense)
        {
            await _licenseService.InsertModelAsync(cuLicense);
            return Ok();
        }

        [HttpPut("model/{licenseId}")]
        public async Task<IActionResult> UpdateModelAsync([FromRoute] Guid licenseId, CULicense cuLicense)
        {
            await _licenseService.UpdateModelAsync(licenseId, cuLicense);
            return Ok();
        }
    }
}
