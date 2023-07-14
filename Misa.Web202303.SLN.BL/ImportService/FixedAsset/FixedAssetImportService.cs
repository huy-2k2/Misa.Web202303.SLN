using AutoMapper;
using Misa.Web202303.QLTS.BL.DomainService.FixedAsset;
using Misa.Web202303.QLTS.BL.Service.Dto;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAssetCategory;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;

namespace Misa.Web202303.QLTS.BL.ImportService.FixedAsset
{
    /// <summary>
    /// định nghĩa các phương thức abstract của baseImportService, override phương thức, định nghĩa các phương thức riêng
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public class FixedAssetImportService : BaseImportService<FixedAssetImportDto, FixedAssetEntity>, IFixedAssetImportService
    {
        #region
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

        private readonly IFixedAssetDomainService _fixedAssetDomainServicecs;
        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo, khởi tạo cho BaseImportService
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="departmentRepository"></param>
        /// <param name="fixedAssetCategoryRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetImportService(IFixedAssetRepository fixedAssetRepository, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository, IUnitOfWork unitOfWork, IMapper mapper, IFixedAssetDomainService fixedAssetDomainServicecs) : base(fixedAssetRepository, unitOfWork)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository = fixedAssetCategoryRepository;
            _mapper = mapper;
            _fixedAssetDomainServicecs = fixedAssetDomainServicecs;
        }
        #endregion

        #region
        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity">danh sách tài nguyên ở dạng importDto</param>
        /// <returns>danh sách tài nguyên ở dạng entity của tầng DL</returns>
        protected override async Task<List<FixedAssetEntity>> MapToListEntity(IEnumerable<FixedAssetImportDto> listImportEntity)
        {
            var departments = await _departmentRepository.GetAsync();
            var fixedAssetCategories = await _fixedAssetCategoryRepository.GetAsync();
            var result = new List<FixedAssetEntity>();
            for (int i = 0; i < listImportEntity.Count(); i++)
            {
                var importEntity = listImportEntity.ElementAt(i);
                // lẩy ra department tương ứng
                var department = departments.Where(d => d.department_code == importEntity.department_code).First();
                // lấy ra fixedAssetCategory tướng ứng
                var fixedAssetCategory = fixedAssetCategories.Where(fac => fac.fixed_asset_category_code == importEntity.fixed_asset_category_code).First();
                var entity = _mapper.Map<FixedAssetEntity>(importEntity);
                // gán departmentId cho entity
                entity.department_id = department.department_id;
                // gán fixedAssetCategoryId cho entity
                entity.fixed_asset_category_id = fixedAssetCategory.fixed_asset_category_id;
                // tạo mới id
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
        protected override List<ValidateError> ValidateBusiness(FixedAssetImportDto entityImportDto)
        {
            var entity = _mapper.Map<FixedAssetEntity>(entityImportDto);
            // dùng phương thức static BusinessValidate của FixedAssetService
            var result = _fixedAssetDomainServicecs.BusinessValidate(entity);
            return result;
        }


        /// <summary>
        /// validate khóa ngoại
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listEntity">danh sách tài nguyên</param>
        /// <param name="errorOfTable">danh sách lỗi trước đó</param>
        /// <returns>danh sách lỗi</returns>
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
                var department = departments.Where(d => d.department_code == entity.department_code);
                // lấy ra fixedAssetCatorygy tướng ụng từ code
                var fixedAssetCategory = fixedAssetCategories.Where(fac => fac.fixed_asset_category_code == entity.fixed_asset_category_code);

                // nếu department không tồn tại thì add thêm lỗi
                if (department.Count() == 0)
                {
                    error.Add(new ValidateError()
                    {
                        FieldNameError = "department_code",
                        Message = string.Format(ErrorMessage.NotExistedError, FieldName.DepartmentCode)
                    });
                }
                // nếu fixedAssetCategory không tồn tại thì add thêm lỗi
                if (fixedAssetCategory.Count() == 0)
                {
                    error.Add(new ValidateError()
                    {
                        FieldNameError = "fixed_asset_category_code",
                        Message = string.Format(ErrorMessage.NotExistedError, FieldName.FixedAssetCategoryCode)
                    });
                }
                result.Add(error);
            }

            return result;

        }
        #endregion
    }
}
