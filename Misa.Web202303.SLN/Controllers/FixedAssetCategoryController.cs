﻿using Microsoft.AspNetCore.Mvc;
using Misa.Web202303.SLN.BL.Service;
using Misa.Web202303.SLN.BL.Service.FixedAssetCategory;

namespace Misa.Web202303.SLN.Controllers
{
    /// <summary>
    /// controller nhận các api liên quan đến loại tài sản
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class FixedAssetCategoryController : BaseController<FixedAssetCategoryDto, FixedAssetCategoryUpdateDto, FixedAssetCategoryCreateDto>
    {
        /// <summary>
        /// sử dụng dịch vụ của IFixedAssetCategoryService
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        private readonly IFixedAssetCategoryService _fixedAssetCategoryService;

        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryService"></param>
        public FixedAssetCategoryController(IFixedAssetCategoryService fixedAssetCategoryService) : base(fixedAssetCategoryService)
        {
            _fixedAssetCategoryService = fixedAssetCategoryService;
        }
    }
}
