using AutoMapper;
using Misa.Web202303.QLTS.BL.ImportService;
using Misa.Web202303.QLTS.BL.ImportService.FixedAssetCategory;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEntity = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;


namespace Misa.Web202303.QLTS.BL.Service.FixedAssetCategory
{  /// <summary>
   /// Lớp định nghĩa các dịch vụ của FixedAssetCategory, gồm các phương thức của IFixedAssetCategoryService, IBaseService, sử dụng lại các phương thức của BaseService
   /// created by: nqhuy(21/05/2023)
   /// </summary>
    public class FixedAssetCategoryService : BaseService<FixedAssetCategoryEntity, FixedAssetCategoryDto, FixedAssetCategoryUpdateDto, FixedAssetCategoryCreateDto>, IFixedAssetCategoryService
    {
        #region
        private readonly IFixedAssetCategoryImportService _fixedAssetCategoryImportService;
        #endregion

        #region
        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryRepository">fixedAssetCategoryRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="fixedAssetCategoryImportService">fixedAssetCategoryImportService</param>
        public FixedAssetCategoryService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper, IFixedAssetCategoryImportService fixedAssetCategoryImportService) : base(fixedAssetCategoryRepository, mapper)
        {
            _fixedAssetCategoryImportService = fixedAssetCategoryImportService;
        }
        #endregion

        #region
        /// <summary>
        /// validate khi thêm loại tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto">dữ liệu tài sản cần validate</param>
        /// <returns>danh sách lỗi</returns>
        protected override async Task<List<ValidateError>> CreateValidateAsync(FixedAssetCategoryCreateDto entityCreateDto)
        {
            // validate nghiệp vụ
            var businessErrors = BusinessValidate(_mapper.Map<FixedAssetCategoryEntity>(entityCreateDto));

            // kiểm tra mã code trùng
            var duplicateErrors = await base.CreateValidateAsync(entityCreateDto);
           
            var listError = Enumerable.Concat(businessErrors, duplicateErrors).ToList();
            
            return listError;
        }

        /// <summary>
        /// validate khi update tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài sản</param>
        /// <param name="entityUpdateDto">dữ liệu tài sản cần valdiate</param>
        /// <returns>danh sách lỗi</returns>
        protected async override Task<List<ValidateError>> UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto entityUpdateDto)
        {
            // validate nghiệp vụ
            var businessErrors = BusinessValidate(_mapper.Map<FixedAssetCategoryEntity>(entityUpdateDto));

            // kiểm tra mã code trùng
            var duplicateErrors = await base.UpdateValidateAsync(id, entityUpdateDto);

            var listError = Enumerable.Concat(businessErrors, duplicateErrors).ToList();

            return listError;
        }

        /// <summary>
        /// validate nghiệp vụ chung
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCategory">dữ liệu tài sản để validate nghiệp vụ</param>
        /// <returns>danh sách lỗi</returns>
        internal static List<ValidateError> BusinessValidate(FixedAssetCategoryEntity fixedAssetCategory)
        {
            var listError = new List<ValidateError>();
            var depreciationRate = Math.Round((double)1 / fixedAssetCategory.life_time * 100, 2);

            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if (fixedAssetCategory.depreciation_rate != depreciationRate)
            {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.DepRateLifeTimeError,
                    FieldNameError = "depreciation_rate"
                });
            }
            return listError;
        }


        /// <summary>
        /// import dữ liệu tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream">file import dưới dạng stream</param>
        /// <param name="isSubmit">biến kiểm tra người dùng có đang submit</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns>dữ liệu về file excel và dữ liệu valdiate</returns>
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
        /// <returns>tên tài nguyên</returns>
        protected override string GetAssetName()
        {
            return AssetName.FixedAssetCategory;
        }
        #endregion
    }
}
