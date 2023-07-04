using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Repository
{
    /// <summary>
    /// định nghĩa các phương thức chung của repository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    /// <typeparam name="TEntity">enitty để lấy dữ liệu từ database</typeparam>
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// lấy 1 bản ghi theo id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên cần lấy</param>
        /// <returns>tài nguyên cần lấy</returns>
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// lấy tất cả bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>danh sách bản ghi</returns>
        Task<IEnumerable<TEntity>> GetAsync();

        /// <summary>
        /// cập nhật bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity">dữ liệu bản ghi cập nhật</param>
        /// <returns></returns>
        Task UpdateAsync(Guid entityId, TEntity entity);

        /// <summary>
        /// thêm bản ghi
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="entity">dữ liệu bản ghi thêm mới</param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// kiểm tra mã code đã tồn tại khi thêm hoạc sửa
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="code">mã code</param>
        /// <param name="id">id (là rỗng trong trường hợp thêm mới)</param>
        /// <returns>false nếu tài nguyên không tồn tại, true nếu tồn tại</returns>
        Task<bool> CheckCodeExistedAsync(string code, Guid? id);

        /// <summary>
        /// xóa nhiều bản ghi dựa vào chuỗi danh sách id, tách nhau bởi dấu ","
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId">danh sách id</param>
        /// <returns></returns>
        Task DeleteListAsync(string listId);

        /// <summary>
        /// lấy ra tổng số bản ghi tồn tại trong danh sách chuỗi id
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listId">danh sách id</param>
        /// <returns>số bản ghi tồn tại trong danh sách chuỗi id</returns>
        Task<int> GetSumExistedOfListAsync(string listId);


        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>tên của table ứng với repository</returns>
        string GetTableName();

        /// <summary>
        /// thêm nhiều bản ghi cùng lúc, dùng cho khi import file
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listEntity">danh sách tài nguyên cần thêm</param>
        /// <returns></returns>
        Task InsertListAsync(IEnumerable<TEntity> listEntity);

        /// <summary>
        /// lấy dữ liệu import của table, column
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>dữ liệu nhập khẩu về cột và bảng</returns>
        Task<IEnumerable<ImportEntity>> GetImportDataAsync();

        /// <summary>
        /// kiểm tra các mã code tồn tại trong listCode khi thêm nhiều tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listCode">danh sách mã tài sản</param>
        /// <returns>danh sánh mã tài sản tồn tại</returns>
        Task<IEnumerable<string>> GetListExistedCodeAsync(string listCode);

        /// <summary>
        /// lấy mã tài nguyên có cùng tiền tố với mã tài nguyên được thêm hoạc sửa gần nhất và có hậu tố lớn nhất
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>danh sách chứa mã tài nguyên và tiền tố</returns>
        Task<List<string>> GetRecommendCodeAsync();
    }
}
