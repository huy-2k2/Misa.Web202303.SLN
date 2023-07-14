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
        private readonly IFixedAssetRepository _fixedAssetRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IMapper _mapper;
        public FixedAssetDomainService(ILicenseRepository licenseRepository, IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _licenseRepository = licenseRepository;
            _mapper = mapper;
        }

        public async Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
            var fixedAssetEntity = _mapper.Map<FixedAssetEntity>(fixedAssetCreateDto);

            var businessErrors = BusinessValidate(fixedAssetEntity);

            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAssetEntity);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();

            var isCodeExisted = await _fixedAssetRepository.CheckCodeExistedAsync(fixedAssetCreateDto.fixed_asset_code, null);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    FieldNameError = $"{_fixedAssetRepository.GetTableName()}_code",
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))

                };
            }
        }

        public async Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            var fixedAssetEntity = _mapper.Map<FixedAssetEntity>(fixedAssetUpdateDto);

            var businessErrors = BusinessValidate(fixedAssetEntity);

            var foreignKeyErrors = await ForeignKeyValidateAsync(fixedAssetEntity);

            var listError = Enumerable.Concat(businessErrors, foreignKeyErrors).ToList();

            var isCodeExisted = await _fixedAssetRepository.CheckCodeExistedAsync(fixedAssetUpdateDto.fixed_asset_code, fixedAssetId);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            var isFixedAssetExisted = await _fixedAssetRepository.GetAsync(fixedAssetId) != null;
            if (!isFixedAssetExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.FixedAsset),
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))

                };
            }
        }

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
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = string.Join("", listError.Select(error => $"<span>{error.Message}</span>"))

                };
            }
        }

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


            var listExisted = await _fixedAssetRepository.GetListExistedAsync(string.Join(",", input.ListIdSelected));

            if (listExisted.Count() != input.ListIdSelected.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InternalError, FieldName.FixedAsset),
                });
            }

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
    }
}
