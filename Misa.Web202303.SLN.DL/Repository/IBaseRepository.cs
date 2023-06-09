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
        Task UpdateAsync(Guid entityId, TEntity entity);

        /// <summary>
        /// thêm bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// kiểm tra mã code đã tồn tại khi thêm hoạc sửa
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CheckCodeExistedAsync(string code, Guid? id);

        /// <summary>
        /// xóa nhiều bản ghi dựa vào chuỗi danh sách id, tách nhau bởi dấu ","
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        Task DeleteListAsync(string listId);

        /// <summary>
        /// lấy ra tổng số bản ghi tồn tại trong danh sách chuỗi id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        Task<int> GetSumExistedOfListAsync(string listId);


        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        string GetTableName();

        /// <summary>
        /// thêm nhiều bản ghi cùng lúc, dùng cho khi import file
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        Task InsertListAsync(IEnumerable<TEntity> listEntity);

        /// <summary>
        /// lấy dữ liệu import của table, column
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ImportEntity>> GetImportDataAsync();

        /// <summary>
        /// kiểm tra các mã code tồn tại trong listCode khi thêm nhiều tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetListExistedCodeAsync(string listCode);
    }
}
