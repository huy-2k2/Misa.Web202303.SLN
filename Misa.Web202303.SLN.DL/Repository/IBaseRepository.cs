using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Repository
{
    /// <summary>
    /// định nghĩa các phương thức chung của repository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// phương thức mở kết nối
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<DbConnection> GetOpenConnectionAsync();

        /// <summary>
        /// lấy 1 bản ghi theo id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// lấy tất cả bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync();

        /// <summary>
        /// cập nhật bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(Guid entityId, TEntity entity);

        /// <summary>
        /// xóa bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// thêm bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);
    }
}
