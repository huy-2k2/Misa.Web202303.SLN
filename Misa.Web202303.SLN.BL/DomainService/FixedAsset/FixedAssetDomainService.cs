using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Misa.Web202303.QLTS.BL.BodyRequest;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.DL.Repository.LicenseDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.DomainService.FixedAsset
{
    public class FixedAssetDomainService : IFixedAssetDomainService
    {
        /// <summary>
        /// repo của fixed asset
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// repo của department
        /// </summary>
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// repo của fixed asset category
        /// </summary>
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;

        /// <summary>
        /// repo của license 
        /// </summary>
        private readonly ILicenseRepository _licenseRepository;

        /// <summary>
        /// repo của license detail
        /// </summary>
        private readonly ILicenseDetailRepository _licenseDetailRepository;

        /// <summary>
        /// mapper để map các đối tượng
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="licenseDetailRepository">licenseDetailRepository</param>
        /// <param name="licenseRepository">licenseRepository</param>
        /// <param name="fixedAssetRepository">fixedAssetRepository</param>
        /// <param name="departmentRepository">departmentRepository</param>
        /// <param name="fixedAssetCategoryRepository">fixedAssetCategoryRepository</param>
        /// <param name="mapper">mapper</param>
        public FixedAssetDomainService(ILicenseDetailRepository licenseDetailRepository, ILicenseRepository licenseRepository, IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _licenseRepository = licenseRepository;
            _licenseDetailRepository = licenseDetailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// hàm validate khi create
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetCreateDto">đối tượng FixedAssetCreateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
            var fixedAssetEntity = _mapper.Map<FixedAssetEntity>(fixedAssetCreateDto);

            // validate business
            var businessErrors = BusinessValidate(fixedAssetEntity);

            // validate foreign key
            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAssetEntity);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();

            // validate mã trùng
            var isCodeExisted = await _fixedAssetRepository.CheckCodeExistedAsync(fixedAssetCreateDto.fixed_asset_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = $"{_fixedAssetRepository.GetTableName()}_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.DataError

                };
            }
        }

        /// <summary>
        /// hàm valdiate khi update 
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetId">id của tài sản</param>
        /// <param name="fixedAssetUpdateDto">đối tượng FixedAssetUpdateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            var fixedAssetEntity = _mapper.Map<FixedAssetEntity>(fixedAssetUpdateDto);
            
            // validate business
            var businessErrors = BusinessValidate(fixedAssetEntity);

            //vavlidate foreign key
            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAssetEntity);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();

            // validate mã trùng
            var isCodeExisted = await _fixedAssetRepository.CheckCodeExistedAsync(fixedAssetUpdateDto.fixed_asset_code, fixedAssetId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // kiểm tra xem id tài sản có thực sự tồn tại không
            var isFixedAssetExisted = await _fixedAssetRepository.GetAsync(fixedAssetId) != null;
            if (!isFixedAssetExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.FixedAsset),
                });
            }

            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.DataError

                };
            }
        }

        /// <summary>
        /// validate khi filter tài sản ở trang tài sản
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="pageSize">kích thước page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <param name="departmentId">id phòng ban</param>
        /// <param name="fixedAssetCategoryId">id của loại tài sản</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task ValidateInputFilterAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId)
        {
            // validate dữ liệu
            // pageSize lớn hơn 0
            var listError = new List<ValidateError>();
            if (pageSize <= 0)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.PageSize),
                });
            }
            // current page lớn hơn 0
            if (currentPage <= 0)
            {
                listError.Add(new ValidateError()
                {
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
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                    });
                }
            }
            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.DataError

                };
            }
        }

        /// <summary>
        /// validate foreign key
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAsset">entity FixedAsset</param>
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
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.DepartmentCode),
                });
            }
            // kiểm tra fixed_asset_category_id tồn tại
            var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync(fixedAsset.fixed_asset_category_id);
            if (fixedAssetCategory == null)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAssetCategoryCode),
                });
            }

            return listError;
        }


        /// <summary>
        /// hàm validate business
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAsset">entitty FixedAsset</param>
        /// <returns>danh sách lỗi</returns>
        public List<ValidateError> BusinessValidate(FixedAssetEntity fixedAsset)
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
            if (fixedAsset.use_date < fixedAsset.purchase_date)
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
        /// hàm validate khi filter lúc chọn tài sản cho chứng từ
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="input">đối tượng chứa dữ liệu đầu vào để filter</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task GetFilterNoLicenseValidateAsync(FilterFixedAssetNoLicense input)
        {
            // validate dữ liệu
            // pageSize lớn hơn 0
            var listError = new List<ValidateError>();
            if (input.PageSize <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "pageSize",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.PageSize),
                });
            }
            // current page lớn hơn 0
            if (input.CurrentPage <= 0)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = "currentPage",
                    Message = string.Format(ErrorMessage.PositiveNumberError, FieldName.CurrentPage),
                });
            }

            // validate xem danh sách id tài sản truyền lên có cái nào không tồn tại hay không
            var listExisted = await _fixedAssetRepository.GetListExistedAsync(string.Join(",", input.ListIdSelected));

            if (listExisted.Count() != input.ListIdSelected.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InternalError, FieldName.FixedAsset),
                });
            }

            // kiểm tra xem license id có thực sự tồn tại không
            if (input.LicenseId != null)
            {
                var license = await _licenseRepository.GetAsync((Guid)input.LicenseId);
                if (license == null)
                {
                    listError.Add(new ValidateError()
                    {

                        Message = string.Format(ErrorMessage.InternalError, FieldName.License),
                    });
                }
            }

            // throw exception nếu có lỗi
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateFilterError
                };
            }
        }

        /// <summary>
        /// hàm validate khí xóa nhiều
        /// created by NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listId">danh sách id xóa</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception nếu có lỗi</exception>
        public async Task DeleteListValidateAsync(IEnumerable<Guid> listId)
        {

            // nối list id lại thành string
            var stringIds = string.Join(",", listId);

            var listError = new List<ValidateError>();

            var listFixedAssetExisted = await _fixedAssetRepository.GetListExistedAsync(stringIds);

            // kiểm tra xem có tài sản nào không tồn tại hay không

            if(listFixedAssetExisted.Count() != listId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset),
                });
            }

            // nếu lỗi dữ liệu thì throw exception
            if(listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data= listError,
                    UserMessage = ErrorMessage.DataError,
                };
            }

            // lấy các tài sản đã phát sinh chứng từ ghi tăng
            var listExisted = await _licenseDetailRepository.GetListFAExistedAsync(stringIds);

            // lấy ra id của danh sách tài sản đã phát sinh chứng từ ghi tăng
            var listFaIdExisted = listExisted.Select(item => item.fixed_asset_id);

            if (listFaIdExisted.Count() > 0)
            {
                // throw ra lỗi khi có 1 tài sản phát sinh
                if (listFaIdExisted.Count() == 1)
                {
                    var fixedAsset = await _fixedAssetRepository.GetAsync(listId.First());
                    var license = await _licenseRepository.GetAsync(listExisted.First().license_id);
                    throw new ValidateException()
                    {
                        Data = new
                        {
                            fixedAsset = fixedAsset,
                            license = license
                        },
                        ErrorCode = ErrorCode.DeleteDetail,
                        UserMessage = ErrorMessage.DataError,
                    };
                }
                // throw ra lỗi khi có nhiều tài sản phát sinh
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
        }
    }
}
