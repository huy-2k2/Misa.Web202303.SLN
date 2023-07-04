    using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetCategoryEntity = Misa.Web202303.QLTS.DL.Entity.FixedAssetCategory;

namespace Misa.Web202303.QLTS.BL.ImportService.FixedAssetCategory
{
    /// <summary>
    /// định nghĩa các phương thức abstract của baseImportService, override phương thức, định nghĩa các phương thức riêng
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public class FixedAssetCategoryImportService : BaseImportService<FixedAssetCategoryImportDto, FixedAssetCategoryEntity>, IFixedAssetCategoryImportService
    {
        #region
        /// <summary>
        /// sử dụng để map từ import dto sang entity
        /// </summary>
        private readonly IMapper _mapper;
        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo
        /// </summary>
        /// <param name="fixedAssetCategoryRepository">fixedAssetCategoryRepository</param>
        /// <param name="mapper">mapper</param>
        public FixedAssetCategoryImportService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper) : base(fixedAssetCategoryRepository)
        {
            _mapper = mapper;
        }
        #endregion

        #region
        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity">danh sách tài nguyên ở dạng importDto</param>
        /// <returns>danh sách tài nguyên ở dạng entity của tầng DL</returns>
        protected override async Task<List<FixedAssetCategoryEntity>> MapToListEntity(IEnumerable<FixedAssetCategoryImportDto> listImportEntity)
        {
            var result = new List<FixedAssetCategoryEntity>();
            foreach (var importEntity in listImportEntity)
            {
                var entity = _mapper.Map<FixedAssetCategoryEntity>(importEntity);
                // thêm guid cho đối tượng
                entity.fixed_asset_category_id = Guid.NewGuid();
                result.Add(entity);
            }

            return result;
        }


        /// <summary>
        /// validate nghiệp vụ 
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityImportDto">tài nguyên cần validate</param>
        /// <returns>danh sách lỗi</returns>
        protected override List<ValidateError> ValidateBusiness(FixedAssetCategoryImportDto entityImportDto)
        {
            var entity = _mapper.Map<FixedAssetCategoryEntity>(entityImportDto);
            var result = FixedAssetCategoryService.BusinessValidate(entity);
            return result;
        }
        #endregion
    }
}
