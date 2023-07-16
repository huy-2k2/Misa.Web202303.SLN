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
        private readonly IMapper _mapper;

        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;
        public FixedAssetCategoryDomainService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _fixedAssetCategoryRepository= fixedAssetCategoryRepository;
        }

        public async Task CreateValidateAsync(FixedAssetCategoryCreateDto fixedAssetCategoryCreateDto)
        {
            var tableName = _fixedAssetCategoryRepository.GetTableName();
            var entity = _mapper.Map<FixedAssetCategoryEntity>(fixedAssetCategoryCreateDto);
            var listError = BusinessValidate(entity);

            var isCodeExisted = await _fixedAssetCategoryRepository.CheckCodeExistedAsync(fixedAssetCategoryCreateDto.fixed_asset_category_code, null);
            if(isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }
            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateCreateError
                };
            }
        }

        public async Task UpdateValidateAsync(Guid id, FixedAssetCategoryUpdateDto fixedAssetCategoryUpdateDto)
        {
            var tableName = _fixedAssetCategoryRepository.GetTableName();
            var entity = _mapper.Map<FixedAssetCategoryEntity>(fixedAssetCategoryUpdateDto);
            var listError = BusinessValidate(entity);

            var isCodeExisted = await _fixedAssetCategoryRepository.CheckCodeExistedAsync(fixedAssetCategoryUpdateDto.fixed_asset_category_code, id);
            if (isCodeExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.DuplicateCodeError, FieldName.CommonCode),
                });
            }

            var isFixedAssetExisted = await _fixedAssetCategoryRepository.GetAsync(id) != null;
            if (!isFixedAssetExisted)
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.NotFoundError, AssetName.FixedAssetCategory),
                });
            }

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateUpdateError
                };
            }
        }

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
