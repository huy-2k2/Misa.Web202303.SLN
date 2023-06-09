using AutoMapper;
using Misa.Web202303.SLN.BL.ImportService;
using Misa.Web202303.SLN.BL.ImportService.FixedAssetCategory;
using Misa.Web202303.SLN.BL.ValidateDto;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
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

        private readonly IFixedAssetCategoryImportService _fixedAssetCategoryImportService;
        public FixedAssetCategoryService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper, IFixedAssetCategoryImportService fixedAssetCategoryImportService) : base(fixedAssetCategoryRepository, mapper)
        {
            _fixedAssetCategoryImportService = fixedAssetCategoryImportService;
        }

        /// <summary>
        /// validate khi thêm loại tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected override async Task<List<ValidateError>> CreateValidateAsync(FixedAssetCategoryCreateDto entityCreateDto)
        {
            var listError = new List<ValidateError>();
            // validate nghiệp vụ
            var commonErrors = BusinessValidate(_mapper.Map<FixedAssetCategoryEntity>(entityCreateDto));
            listError = Enumerable.Concat(listError, commonErrors).ToList();
            // kiểm tra mã code trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityCreateDto.Fixed_asset_category_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "fixed_asset_category_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAssetCategoryCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// validate khi update tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task<List<ValidateError>> UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto entityUpdateDto)
        {
            var listError = new List<ValidateError>();
            // validate nghiệp vụ
            var commonErrors = BusinessValidate(_mapper.Map<FixedAssetCategoryEntity>(entityUpdateDto));

            listError = Enumerable.Concat(listError, commonErrors).ToList();

            // kiểm tra mã code trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(entityUpdateDto.Fixed_asset_category_code, id);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "fixed_asset_category_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.FixedAssetCategoryCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// validate nghiệp vụ chung
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategory"></param>
        /// <exception cref="ValidateException"></exception>
        public static List<ValidateError> BusinessValidate(FixedAssetCategoryEntity fixedAssetCategory)
        {
            var listError = new List<ValidateError>();
            var depreciationRate = Math.Round((double)1 / fixedAssetCategory.Life_time * 100, 2);

            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if (fixedAssetCategory.Depreciation_rate != depreciationRate)
            {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.DepRateLifeTimeError,
                    FieldNameError = "Depreciation_rate"
                });
            }
            return listError;
        }


        /// <summary>
        /// import dữ liệu loại tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isSubmit"></param>
        /// <returns></returns>
        public async Task<ImportErrorEntity<FixedAssetCategoryEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit)
        {

            // validate dữ liệu
            var validateEntity = await _fixedAssetCategoryImportService.ValidateAsync(stream);
            if (isSubmit && validateEntity.IsPassed || !isSubmit)
            {
                var listEntity = validateEntity.ListEntity;
                // nếu không có lỗi thì import
                if (isSubmit)
                    await _baseRepository.InsertListAsync(listEntity);
                return validateEntity;
            }
            else
            {
                // có lỗi thì throw exception
                throw new ValidateException()
                {
                    Data = validateEntity,
                    ErrorCode = ErrorCode.InvalidData,
                    UserMessage = ErrorMessage.FileDataError
                };
            }
        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override string GetAssetName()
        {
            return AssetName.FixedAssetCategory;
        }
    }
}
