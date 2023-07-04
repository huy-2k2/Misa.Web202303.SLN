using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.ImportService;
using Misa.Web202303.QLTS.BL.ImportService.FixedAsset;
using Misa.Web202303.QLTS.BL.RecommendCode;
using Misa.Web202303.QLTS.BL.Service.Dto;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.filter;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

// vì FixedAsset trong Entity ở DL trùng với tên của namespace nên đặt bí danh
using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.Service.FixedAsset
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của FixedAsset, gồm các phương thức của IFixedAssetService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary
    public class FixedAssetService : BaseService<FixedAssetEntity, FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>, IFixedAssetService
    {
        #region
        /// <summary>
        /// sử dụng để gọi phương thức của IFixedAssetRepository
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// gọi phương thức của IDepartmentRepository
        /// </summary>
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// gọi phương thức của IFixedAssetCategoryRepository
        /// </summary>
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;

        /// <summary>
        /// sử dụng để gọi phương thức import từ file exccel
        /// </summary>
        private readonly IFixedAssetImportService _fixedAssetImportService;

        /// <summary>
        /// sử dụng để tạo mã code mới
        /// </summary>
        private readonly IRecommendCodeService _recommendCodeService;
        #endregion


        #region

        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetRepository">fixedAssetRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="departmentRepository">departmentRepository</param>
        /// <param name="fixedAssetCategoryRepository">fixedAssetCategoryRepository</param>
        /// <param name="fixedAssetImportService">fixedAssetImportService</param>
        /// <param name="recommendCodeService">recommendCodeService</param>
        /// 
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IMapper mapper, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IFixedAssetImportService fixedAssetImportService, IRecommendCodeService recommendCodeService) : base(fixedAssetRepository, mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _fixedAssetImportService = fixedAssetImportService;
            _recommendCodeService= recommendCodeService;
        }
        #endregion

        #region


        /// <summary>
        /// filter, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="departmentId">mã phòng ban</param>
        /// <param name="fixedAssetCategoryId">mã loại tài sản</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns>danh sách tài sản thỏa mãn yêu cầu filter, phân trang</returns>
        public async Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            // validate dữ liệu
            // pageSize lớn hơn 0
            var listError = new List<ValidateError>();
            if (pageSize <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "pageSize",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.PageSize),
                });
            }
            // current page lớn hơn 0
            if (currentPage <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "currentPage",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.CurrentPage),
                });
            }
            // department tồn tại
            if (departmentId != null)
            {
                var department = await _departmentRepository.GetAsync((Guid)departmentId);
                if (department == null)
                {
                    listError.Add(new ValidateError()
                    {
                        FieldNameError = "department_id",
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.DepartmentCode),
                    });
                }
            }
            // loại tài sản tồn tại
            if (fixedAssetCategoryId != null)
            {
                var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync((Guid)fixedAssetCategoryId);
                if (fixedAssetCategory == null)
                {
                    listError.Add(new ValidateError()
                    {
                        FieldNameError = "fixed_asset_category_id",
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                    });
                }
            }
            // nếu có lỗi thì throw excception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateFilterError
                };
            }
            var filterListFixedAsset = await _fixedAssetRepository.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
            return filterListFixedAsset;
        }


        /// <summary>
        /// tự động sinh mà tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>mã tài sản gợi ý khi thêm mới</returns>
        public async Task<string> GetRecommendFixedAssetCodeAsync()
        {
            // lấy ra tiền tố và mã tài sản có hậu tố lớn nhất
            var tempList = await _fixedAssetRepository.GetRecommendCodeAsync();
            // lấy ra tiền tố ở ví trị 0
            var prefixCode = tempList.First();
            // lấy ra hậu tố sản ở vị trí 1
            var postFix = tempList.Last();

            var newCode = _recommendCodeService.CreateRecommendCode(prefixCode, postFix);
            return newCode;
        }


        /// <summary>
        /// validate khi thêm mới dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCreateDto">dữ liệu fixed asset để valdiate</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns>danh sách lỗi</returns>
        protected async override Task<List<ValidateError>> CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
           
            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetCreateDto);
            
            // validate nghiệp vụ
            var businessErrors = BusinessValidate(fixedAsset);

            // validate khóa ngoại
            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAsset);

            // kiểm tra mã tài sản bị trùng
            var duplicateErrors = await base.CreateValidateAsync(fixedAssetCreateDto);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();
            listError = listError.Concat(duplicateErrors).ToList();
            return listError;
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <param name="fixedAssetUpdateDto">dữ liệu tài sản cần validate</param>
        /// <returns>danh sách lỗi</returns>

        protected async override Task<List<ValidateError>> UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {

            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetUpdateDto);

            // validate nghiệp vụ
            var businessErrors = BusinessValidate(fixedAsset);

            // validate khóa ngoại
            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAsset);

            // kiểm tra trùng mã
            var duplicateErrors = await base.UpdateValidateAsync(fixedAssetId, fixedAssetUpdateDto);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();

            listError = listError.Concat(duplicateErrors).ToList();


            return listError;
        }

        /// <summary>
        /// hàm validate chung cho cả insert và update, thực hiện các logic nghiệp vụ
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        /// <returns>danh sách lỗi</returns>
        private async Task<List<ValidateError>> ForeignKeyValidateAsync(FixedAssetEntity fixedAsset)
        {
            var listError = new List<ValidateError>();
            // kiểm tra department_id tồn tại
            var department = await _departmentRepository.GetAsync(fixedAsset.department_id);
            if (department == null)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "DepartmentCode",
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.DepartmentCode),
                });
            }
            // kiểm tra fixed_asset_category_id tồn tại
            var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync(fixedAsset.fixed_asset_category_id);
            if (fixedAssetCategory == null)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "fixed_asset_category_id",
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                });
            }
            return listError;
        }

        /// <summary>
        /// validate nghiệp vụ
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAsset">dữ liệu tài sản cần validate</param>
        /// <returns>danh sách lỗi</returns>
        internal static List<ValidateError> BusinessValidate(FixedAssetEntity fixedAsset)
        {
            var listError = new List<ValidateError>();
            // làm tròn số
            var depreciationAnnual = Math.Round((double)fixedAsset.depreciation_rate * fixedAsset.cost / 100, 2);
            var depreciationRate = Math.Round((double)1 / fixedAsset.life_time * 100, 2);
            // hao mòn năm  = tỷ lệ hao mòn * nguyên giá
            if (fixedAsset.depreciation_annual != depreciationAnnual)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "depreciation_annual",
                    Message = ErrorMessage.DepAnnualCostDepRateError,
                });
            }
            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if (fixedAsset.depreciation_rate != depreciationRate)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "depreciation_rate",
                    Message = ErrorMessage.DepRateLifeTimeError,
                });
            }
            // ngày mua nhỏ hơn hoạc bằng ngày sử dụng
            if(fixedAsset.use_date < fixedAsset.purchase_date)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "use_date",
                    Message = ErrorMessage.UseDateLessThanPurchaseDateError,
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
        public async Task<ImportErrorEntity<FixedAssetEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit)
        {
            // validate dữ liệu
            var validateEntity = await _fixedAssetImportService.ValidateAsync(stream);
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
            return AssetName.FixedAsset;
        }

         /// <summary>
        ///  lấy danh sách tài sản thoải mãn điều kiện filter và phân trang, chưa có chứng từ
        /// </summary>
        /// <param name="filterFixedAssetNoLicense">đối tượng chứa dữ liệu filter</param>
        /// <returns>danh sách tài sản chưa có chứng từ thỏa mã điều kiện filter</returns>
        public async Task<FilterListFixedAsset> GetFilterNotHasLicenseAsync(FilterFixedAssetNoLicense filterFixedAssetNoLicense)
        {
           // validate

            var listIdString = string.Join(",", filterFixedAssetNoLicense.ListIdSelected);

            var result = await _fixedAssetRepository.GetFilterNotHasLicenseAsync(filterFixedAssetNoLicense.PageSize, filterFixedAssetNoLicense.CurrentPage, listIdString, filterFixedAssetNoLicense.TextSearch);

            return result;
        }

        public async Task<IEnumerable<FixedAssetDto>> GetListByLicenseIdAsync(Guid licenseId)
        {
            var listEntity = await _fixedAssetRepository.GetListByLicenseId(licenseId);
            var result = listEntity.Select(entity =>
            {
                var fixedAsset = _mapper.Map<FixedAssetDto>(entity);
                return fixedAsset;
            });
            return result;
        }
        #endregion
    }
}
