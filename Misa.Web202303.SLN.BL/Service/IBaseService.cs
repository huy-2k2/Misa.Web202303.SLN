using Misa.Web202303.SLN.BL.Service.FixedAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service
{
    /// <summary>
    /// định nghĩa các phương thức chung của service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TEntityUpdateDto"></typeparam>
    /// <typeparam name="TEntityCreateDto"></typeparam>
    public interface IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto>
    {
        /// <summary>
        /// lấy ra 1 bản thi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId"></param>
        /// <returns></returns>
        Task<TEntityDto> GetAsync(Guid fixedAssetId);

        /// <summary>
        /// lấy ra tất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntityDto>> GetAsync();

        /// <summary>
        /// thêm 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        Task<TEntityDto> InsertAsync(TEntityCreateDto entityDto);

        /// <summary>
        /// phương thức update 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        Task<TEntityDto> UpdateAsync(Guid entityId, TEntityUpdateDto entityDto);

        /// <summary>
        /// xóa 1 bản ghi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// kiểm tra mã code bị trùng khi thêm hoạc sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CheckCodeExisted(string code, Guid? id);
    }
}
