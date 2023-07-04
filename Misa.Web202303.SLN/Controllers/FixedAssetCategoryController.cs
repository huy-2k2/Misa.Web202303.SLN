using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.QLTS.BL.Service;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using System.Net;

namespace Misa.Web202303.QLTS.API.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến loại tài sản
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryController : BaseController<FixedAssetCategoryDto, FixedAssetCategoryUpdateDto, FixedAssetCategoryCreateDto>
    {
        #region
        /// <summary>
        /// sử dụng dịch vụ của IFixedAssetCategoryService
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        private readonly IFixedAssetCategoryService _fixedAssetCategoryService;
        #endregion


        #region
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryService"></param>
        public FixedAssetCategoryController(IFixedAssetCategoryService fixedAssetCategoryService) : base(fixedAssetCategoryService)
        {
            _fixedAssetCategoryService = fixedAssetCategoryService;
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
                var result = await _fixedAssetCategoryService.ImportFileAsync(stream, isSubmit);
                return StatusCode((int)HttpStatusCode.Created, result);

            }
        }
        #endregion
    }
}
