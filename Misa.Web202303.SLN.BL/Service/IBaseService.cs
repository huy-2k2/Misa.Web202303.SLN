using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service
{
    /// <summary>
    /// định nghĩa các phương thức chung của service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntityDto">dto để lấy dữ liệu</typeparam>
    /// <typeparam name="TEntityUpdateDto">dto để update dữ liệu</typeparam>
    /// <typeparam name="TEntityCreateDto">dto để thêm dữ liệu</typeparam>
    public interface IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto>
    {
        /// <summary>
        /// lấy ra 1 bản thi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên cần lấy</param>
        /// <returns>bản ghi có id là id đã cho</returns>
        Task<TEntityDto> GetAsync(Guid id);

        /// <summary>
        /// lấy ra tất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tất cả tài nguyên trong 1 bảng</returns>
        Task<IEnumerable<TEntityDto>> GetAsync();

        /// <summary>
        /// thêm 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto">dữ liệu tài nguyên thêm mới</param>
        /// <returns></returns>
        Task InsertAsync(TEntityCreateDto entityDto);

        /// <summary>
        /// phương thức update 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// /// <param name="entityId">id tài nguyên cần sửa</param>
        /// <param name="entityDto">dữ liệu tài nguyên cần sửa</param>
        /// <returns></returns>
        Task UpdateAsync(Guid entityId, TEntityUpdateDto entityDto);


        /// <summary>
        /// kiểm tra mã code bị trùng khi thêm hoạc sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code">mã code</param>
        /// <param name="id">id (là rỗng trong trường hợp thêm mới)</param>
        /// <returns></returns>
        Task<bool> CheckCodeExisted(string code, Guid? id);

        /// <summary>
        /// xóa nhiều bản ghi cùng lúc dựa vào danh sách id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listId">danh sách id tài nguyên cần xóa</param>
        /// <returns></returns>
        Task DeleteListAsync(IEnumerable<Guid> listId);
    }
}
