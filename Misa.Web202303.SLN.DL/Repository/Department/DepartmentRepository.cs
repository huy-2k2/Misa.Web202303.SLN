using Microsoft.Extensions.Configuration;
using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.SLN.DL.Entity.Department;

namespace Misa.Web202303.SLN.DL.Repository.Department
{
    /// <summary>
    /// thực thi các phương thức của IDepartmentRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class DepartmentRepository : BaseRepository<DepartmentEntity>, IDepartmentRepository
    {
        /// <summary>
        /// hàm khởi tạo
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration"></param>
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// lấy ra tên của table trong csdl ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "Department";
        }
    }
}
