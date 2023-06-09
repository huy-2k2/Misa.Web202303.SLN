using AutoMapper;
using Misa.Web202303.SLN.BL.Service.Department;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.DL.Repository;
using Misa.Web202303.SLN.DL.Repository.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.BL.ImportService.Department
{
    public class DepartmentImportService : BaseImportService<DepartmentImportDto, DepartmentEntity>, IDepartmentImportService
    {
        /// <summary>
        /// sử dụng để map từ import dto sang entity
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// hàm khởi tạo
        /// </summary>
        /// <param name="departmentRepository"></param>
        /// <param name="mapper"></param>
        public DepartmentImportService(IDepartmentRepository departmentRepository, IMapper mapper) : base(departmentRepository)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// lấy ra danh dánh TEntity từ TEntityImportDto
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listImportEntity"></param>
        /// <returns></returns>
        protected override async Task<List<DepartmentEntity>> MapToListEntity(IEnumerable<DepartmentImportDto> listImportEntity)
        {
            var result = new List<DepartmentEntity>();
            foreach (var importEntity in listImportEntity)
            {
                var entity = _mapper.Map<DepartmentEntity>(importEntity);
                entity.Department_id = Guid.NewGuid();
                result.Add(entity);
            }

            return result;
        }
    }
}
