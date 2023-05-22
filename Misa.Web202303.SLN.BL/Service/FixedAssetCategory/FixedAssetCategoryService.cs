using AutoMapper;
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
        public FixedAssetCategoryService(IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper) : base(fixedAssetCategoryRepository, mapper)
        {
        }
     
    }
}
