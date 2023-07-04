using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.DL.Repository.Department
{
    /// <summary>
    /// thực thi các phương thức của IDepartmentRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class DepartmentRepository : BaseRepository<DepartmentEntity>, IDepartmentRepository
    {
        #region
        /// <summary>
        /// hàm khởi tạo
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region
        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>tên của table department</returns>
        public override string GetTableName()
        {
            return "department";
        }
        #endregion
    }
}
