    using AutoMapper;
using Misa.Web202303.SLN.BL.Service.FixedAssetCategory;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEntity = Misa.Web202303.SLN.DL.Entity.FixedAssetCategory;

namespace Misa.Web202303.SLN.BL.ImportService.FixedAssetCategory
{
    public class FixedAssetCategoryImportService : BaseImportService<FixedAssetCategoryImportDto, FixedAssetCategoryEntity>, IFixedAssetCategoryImportService
    {
        /// <summary>
        /// sử dụng để map từ import dto sang entity
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// hàm khởi tạo
        /// </summary>
        /// <param name="fixedAssetCategoryRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetCategoryImportService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper) : base(fixedAssetCategoryRepository)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity"></param>
        /// <returns></returns>
        protected override async Task<List<FixedAssetCategoryEntity>> MapToListEntity(IEnumerable<FixedAssetCategoryImportDto> listImportEntity)
        {
            var result = new List<FixedAssetCategoryEntity>();
            foreach (var importEntity in listImportEntity)
            {
                var entity = _mapper.Map<FixedAssetCategoryEntity>(importEntity);
                // thêm guid cho đối tượng
                entity.Fixed_asset_category_id = Guid.NewGuid();
                result.Add(entity);
            }

            return result;
        }


        /// <summary>
        /// validate nghiệp vụ 
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override List<ValidateError> ValidateBusiness(FixedAssetCategoryImportDto entityImportDto)
        {
            var entity = _mapper.Map<FixedAssetCategoryEntity>(entityImportDto);
            var result = FixedAssetCategoryService.BusinessValidate(entity);
            return result;
        }
    }
}
