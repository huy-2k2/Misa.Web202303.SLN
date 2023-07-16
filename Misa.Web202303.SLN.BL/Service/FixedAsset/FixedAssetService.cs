using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.DomainService.FixedAsset;
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
using Misa.Web202303.QLTS.DL.Filter;
using Misa.Web202303.QLTS.DL.Model;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.DL.Repository.LicenseDetail;
using Misa.Web202303.QLTS.DL.unitOfWork;
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
using FixedAssetModel = Misa.Web202303.QLTS.DL.Model.FixedAsset;

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

        private readonly IFixedAssetDomainService _fixedAssetDomainService;

        private readonly ILicenseDetailRepository _licenseDetailRepository;

        private readonly ILicenseRepository _licenseRepository;

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
        /// <param name="unitOfWork">unitOfWork</param>
        /// 
        public FixedAssetService(ILicenseRepository licenseRepository, ILicenseDetailRepository licenseDetailRepository, IFixedAssetRepository fixedAssetRepository, IUnitOfWork unitOfWork, IMapper mapper, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IFixedAssetImportService fixedAssetImportService, IRecommendCodeService recommendCodeService, IFixedAssetDomainService fixedAssetDomainServicecs) : base(fixedAssetRepository, unitOfWork, mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _fixedAssetImportService = fixedAssetImportService;
            _recommendCodeService = recommendCodeService;
            _fixedAssetDomainService = fixedAssetDomainServicecs;
            _licenseDetailRepository = licenseDetailRepository;
            _licenseRepository = licenseRepository;
        }
        #endregion

        #region


        /// <summary>
        /// Filter, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="departmentId">mã phòng ban</param>
        /// <param name="fixedAssetCategoryId">mã loại tài sản</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns>danh sách tài sản thỏa mãn yêu cầu Filter, phân trang</returns>
        public async Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            await _fixedAssetDomainService.ValidateInputFilterAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId);
            // nếu có lỗi thì throw excception


            var filterListFixedAsset = await _fixedAssetRepository.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);

            //await _unitOfWork.CommitAsync();

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

            await _unitOfWork.CommitAsync();

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
        protected async override Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
            await _fixedAssetDomainService.CreateValidateAsync(fixedAssetCreateDto);
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId">id tài sản</param>
        /// <param name="fixedAssetUpdateDto">dữ liệu tài sản cần validate</param>
        /// <returns>danh sách lỗi</returns>

        protected async override Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            await _fixedAssetDomainService.UpdateValidateAsync(fixedAssetId, fixedAssetUpdateDto);
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
                await _unitOfWork.CommitAsync();
                return validateEntity;
            }
            else
            {
                await _unitOfWork.CommitAsync();
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
        ///  lấy danh sách tài sản thoải mãn điều kiện Filter và phân trang, chưa có chứng từ
        /// </summary>
        /// <param name="filterFixedAssetNoLicense">đối tượng chứa dữ liệu Filter</param>
        /// <returns>danh sách tài sản chưa có chứng từ thỏa mã điều kiện Filter</returns>
        public async Task<FilterListFixedAsset> GetFilterNotHasLicenseAsync(FilterFixedAssetNoLicense filterFixedAssetNoLicense)
        {
            // validate

            var listIdString = string.Join(",", filterFixedAssetNoLicense.ListIdSelected);

            var result = await _fixedAssetRepository.GetFilterNotHasLicenseAsync(filterFixedAssetNoLicense.PageSize, filterFixedAssetNoLicense.CurrentPage, listIdString, filterFixedAssetNoLicense.TextSearch, filterFixedAssetNoLicense.LicenseId);

            await _unitOfWork.CommitAsync();

            return result;
        }

        public async Task<IEnumerable<FixedAssetModel>> GetListByLicenseIdAsync(Guid licenseId)
        {
            var listEntity = await _fixedAssetRepository.GetListByLicenseId(licenseId);

            await _unitOfWork.CommitAsync();

            return listEntity;
        }

        public override async Task DeleteListAsync(IEnumerable<Guid> listId)
        {
            var stringIds = string.Join(",", listId);

            var listExisted = await _licenseDetailRepository.GetListFAExistedAsync(stringIds);

            var listFaIdExisted = listExisted.Select(item => item.fixed_asset_id);



            if (listFaIdExisted.Count() > 0)
            {

                if (listFaIdExisted.Count() == 1)
                {
                    var fixedAsset = await _fixedAssetRepository.GetAsync(listId.First());
                    var license = await _licenseRepository.GetAsync(listExisted.First().license_id);
                    throw new ValidateException()
                    {
                        Data = new
                        {
                            fixedAsset =  fixedAsset,
                            license = license
                        },
                        ErrorCode = ErrorCode.DeleteDetail,
                        UserMessage = ErrorMessage.DataError,
                    };
                }
                else
                {
                    throw new ValidateException()
                    {
                        Data = listFaIdExisted,
                        ErrorCode = ErrorCode.DeleteDetailMulti,
                        UserMessage = ErrorMessage.DataError,
                    };

                }
               
            }

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    await _fixedAssetRepository.DeleteListAsync(stringIds);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
            }

        }
        #endregion
    }
}
