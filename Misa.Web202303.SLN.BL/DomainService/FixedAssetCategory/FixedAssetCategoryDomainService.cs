using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedAssetCategoryEntity = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;


namespace Misa.Web202303.QLTS.BL.DomainService.FixedAssetCategory
{
    public class FixedAssetCategoryDomainService : IFixedAssetCategoryDomainService
    {
        /// <summary>
        /// mapper để map giữa các đối tượng
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// repo gọi table fixed_asset_category
        /// </summary>
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryRepository">interface IFixedAssetCategoryRepository</param>
        /// <param name="mapper">mapper</param>
        public FixedAssetCategoryDomainService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _fixedAssetCategoryRepository= fixedAssetCategoryRepository;
        }

        /// <summary>
        /// hàm validate khi create 
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="fixedAssetCategoryCreateDto">đối tượng FixedAssetCategoryCreateDto</param>
        /// <exception cref="ValidateException">throw exception khi có lỗi</exception>
        public async Task CreateValidateAsync(FixedAssetCategoryCreateDto fixedAssetCategoryCreateDto)
        {
            // validate business
            var entity = _mapper.Map<FixedAssetCategoryEntity>(fixedAssetCategoryCreateDto);
            var listError = BusinessValidate(entity);

            // kiểm tra mã trùng
            var isCodeExisted = await _fixedAssetCategoryRepository.CheckCodeExistedAsync(fixedAssetCategoryCreateDto.fixed_asset_category_code, null);
            if(isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }
            
            // có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateCreateError
                };
            }
        }

        /// <summary>
        /// hàm validate khi update fixed asset
        /// </summary>
        /// created by: NQ Huy (08/07/2023)
        /// <param name="id">id của đối tượng update</param>
        /// <param name="fixedAssetCategoryUpdateDto">đối tượng FixedAssetCategoryUpdateDto</param>
        /// <exception cref="ValidateException"></exception>
        public async Task UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto fixedAssetCategoryUpdateDto)
        {
            // validate business
            var entity = _mapper.Map<FixedAssetCategoryEntity>(fixedAssetCategoryUpdateDto);
            var listError = BusinessValidate(entity);

            // kiểm tra mã trùng
            var isCodeExisted = await _fixedAssetCategoryRepository.CheckCodeExistedAsync(fixedAssetCategoryUpdateDto.fixed_asset_category_code, id);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            // kiểm tra xem id sửa có tồn tại hay không
            var isFixedAssetCategoryExisted = await _fixedAssetCategoryRepository.GetAsync(id) != null;
            if (!isFixedAssetCategoryExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.FixedAssetCategory),
                });
            }

            // nếu có lỗi thì throw exception
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateUpdateError
                };
            }
        }

        /// <summary>
        /// hàm validate business
        /// created by: NQ Huy (08/07/2023)
        /// </summary>
        /// <param name="fixedAssetCategory">entity FixedAssetCategory</param>
        /// <returns>danh sách lỗi</returns>
        public List<ValidateError> BusinessValidate(FixedAssetCategoryEntity fixedAssetCategory)
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
    }
}
