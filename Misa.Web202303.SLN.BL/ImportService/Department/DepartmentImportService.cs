using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.Department;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.BL.ImportService.Department
{
    /// <summary>
    /// định nghĩa các phương thức abstract của baseImportService, override phương thức, định nghĩa các phương thức riêng
    /// created by: nqhuy(10/06/2023)
    /// </summary>
    public class DepartmentImportService : BaseImportService<DepartmentImportDto, DepartmentEntity>, IDepartmentImportService
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
        /// <param name="departmentRepository"></param>
        /// <param name="mapper"></param>
        public DepartmentImportService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(departmentRepository, unitOfWork)
        {
            _mapper = mapper;
        }
        #endregion

        #region
        // <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity">danh sách tài nguyên ở dạng importDto</param>
        /// <returns>danh sách tài nguyên ở dạng entity của tầng DL</returns>
        protected override async Task<List<DepartmentEntity>> MapToListEntity(IEnumerable<DepartmentImportDto> listImportEntity)
        {
            var result = new List<DepartmentEntity>();
            foreach (var importEntity in listImportEntity)
            {
                var entity = _mapper.Map<DepartmentEntity>(importEntity);
                result.Add(entity);
            }

            return result;
        }
        #endregion
    }
}
