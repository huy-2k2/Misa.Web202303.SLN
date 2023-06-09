using AutoMapper;
using Misa.Web202303.SLN.BL.Service.Dto;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.Department;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.SLN.DL.Entity.FixedAsset;

namespace Misa.Web202303.SLN.BL.ImportService.FixedAsset
{
    public class FixedAssetImportService : BaseImportService<FixedAssetImportDto, FixedAssetEntity>, IFixedAssetImportService
    {
        /// <summary>
        /// sử dụng FixedAssetRepository
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// sử dụng departmentRepository
        /// </summary>
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// sử dụng FixedAssetCategoryRepository
        /// </summary>
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;

        /// <summary>
        /// sử dụng mapper
        /// </summary>
        private readonly IMapper _mapper;
        
        /// <summary>
        /// hàm khởi tạo, khởi tạo cho BaseImportService
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="departmentRepository"></param>
        /// <param name="fixedAssetCategoryRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetImportService(IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IMapper mapper) : base(fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity"></param>
        /// <returns></returns>
        protected override async Task<List<FixedAssetEntity>> MapToListEntity(IEnumerable<FixedAssetImportDto> listImportEntity)
        {
            var departments = await _departmentRepository.GetAsync();
            var fixedAssetCategories = await _fixedAssetCategoryRepository.GetAsync();
            var result = new List<FixedAssetEntity>();
            for(int i = 0; i< listImportEntity.Count(); i++)
            {
                var importEntity = listImportEntity.ElementAt(i);
                // lẩy ra department tương ứng
                var department = departments.Where(d => d.Department_code == importEntity.Department_code).First();
                // lấy ra fixedAssetCategory tướng ứng
                var fixedAssetCategory = fixedAssetCategories.Where(fac => fac.Fixed_asset_category_code == importEntity.Fixed_asset_category_code).First();
                var entity = _mapper.Map<FixedAssetEntity>(importEntity);
                // gán departmentId cho entity
                entity.Department_id = department.Department_id;
                // gán fixedAssetCategoryId cho entity
                entity.Fixed_asset_category_id = fixedAssetCategory.Fixed_asset_category_id;
                // tạo mới id
                entity.Fixed_asset_id = Guid.NewGuid();
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
        protected override List<ValidateError> ValidateBusiness(FixedAssetImportDto entityImportDto)
        {
            var entity = _mapper.Map<FixedAssetEntity>(entityImportDto);
            // dùng phương thức static BusinessValidate của FixedAssetService
            var result = FixedAssetService.BusinessValidate(entity);
            return result;
        }


        /// <summary>
        /// validate khóa ngoại
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="errorOfTable"></param>
        /// <returns></returns>
        protected override async Task<List<List<ValidateError>>> ValidateForeignKeyAsync(IEnumerable<FixedAssetImportDto> listEntity, IEnumerable<IEnumerable<ValidateError>> errorOfTable)
        {
            var departments = await _departmentRepository.GetAsync();
            var fixedAssetCategories = await _fixedAssetCategoryRepository.GetAsync();
            var result = new List<List<ValidateError>>();
            for (int i = 0; i < listEntity.Count(); i++)
            {
                var error = errorOfTable.ElementAt(i).ToList();
                var entity = listEntity.ElementAt(i);
                // lấy ra department tương ứng từ code
                var department = departments.Where(d => d.Department_code == entity.Department_code);
                // lấy ra fixedAssetCatorygy tướng ụng từ code
                var fixedAssetCategory = fixedAssetCategories.Where(fac => fac.Fixed_asset_category_code == entity.Fixed_asset_category_code);

                // nếu department không tồn tại thì add thêm lỗi
                if (department.Count() == 0)
                {
                    error.Add(new ValidateError()
                    {
                        FieldNameError = "Department_code",
                        Message = string.Format(ErrorMessage.NotExistedError, FieldName.DepartmentCode)
                    });
                }
                // nếu fixedAssetCategory không tồn tại thì add thêm lỗi
                if (fixedAssetCategory.Count() == 0)
                {
                    error.Add(new ValidateError()
                    {
                        FieldNameError = "Fixed_asset_category_code",
                        Message = string.Format(ErrorMessage.NotExistedError, FieldName.FixedAssetCategoryCode)
                    });
                }
                result.Add(error);
            }
            return result;

        }
    }
}
