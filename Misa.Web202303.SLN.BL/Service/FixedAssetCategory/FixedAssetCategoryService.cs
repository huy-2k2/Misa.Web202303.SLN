using AutoMapper;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEntity = Misa.Web202303.SLN.DL.Entity.FixedAssetCategory;


namespace Misa.Web202303.SLN.BL.Service.FixedAssetCategory
{  /// <summary>
   /// Lớp định nghĩa các dịch vụ của FixedAssetCategory, gồm các phương thức của IFixedAssetCategoryService, IBaseService, sử dụng lại các phương thức của BaseService
   /// created by: nqhuy(21/05/2023)
   /// </summary>
    public class FixedAssetCategoryService : BaseService<FixedAssetCategoryEntity, FixedAssetCategoryDto, FixedAssetCategoryUpdateDto, FixedAssetCategoryCreateDto>, IFixedAssetCategoryService
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetCategoryService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper) : base(fixedAssetCategoryRepository, mapper)
        {
        }

        /// <summary>
        /// validate khi thêm loại tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected override async Task CreateValidateAsync(FixedAssetCategoryCreateDto entityCreateDto)
        {
            // validate nghiệp vụ
            CommonValidate(_mapper.Map<FixedAssetCategoryEntity>(entityCreateDto));

            // kiểm tra mã code trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityCreateDto.Fixed_asset_category_code, null);
            if (isCodeExisted)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã loại tài sản"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã loại tài sản"),
                    ErrorCode = ErrorCode.DuplicateCode
                };
            }
        }

        /// <summary>
        /// validate khi update tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto entityUpdateDto)
        { 
            // validate nghiệp vụ
            CommonValidate(_mapper.Map<FixedAssetCategoryEntity>(entityUpdateDto));

            // kiểm tra mã code trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityUpdateDto.Fixed_asset_category_code, id);
            if (isCodeExisted)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã loại tài sản"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã loại tài sản"),
                    ErrorCode = ErrorCode.DuplicateCode

                };
            }
        }

        /// <summary>
        /// validate nghiệp vụ chung
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategory"></param>
        /// <exception cref="ValidateException"></exception>
        private void CommonValidate(FixedAssetCategoryEntity fixedAssetCategory)
        {
            var depreciationRate = Math.Round((double)1 / fixedAssetCategory.Life_time * 100, 2);

            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if (fixedAssetCategory.Depreciation_rate != depreciationRate)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DepRateLifeTimeError,
                    DevMessage = ErrorMessage.DepRateLifeTimeError,
                    ErrorCode = ErrorCode.BusinessValidate
                };
            }
        }
    }
}
