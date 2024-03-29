﻿using Misa.Web202303.SLN.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.SLN.DL.Entity.FixedAsset;

namespace Misa.Web202303.SLN.DL.Repository.FixedAsset
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của FixedAssetRepository, ngoài các phương thức của IBaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public interface IFixedAssetRepository : IBaseRepository<FixedAssetEntity>
    {
        /// <summary>
        /// xóa nhiều tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string listFixedAssetId);

        /// <summary>
        /// đếm số tài sản tồn tại trong danh sách tài sản
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        Task<int> CountFixedAssetInListIdAsync(string listFixedAssetId);


        /// <summary>
        /// lấy các mã tài sản có cùng tiền tố với mã tài sản được thêm hoạc sửa gần nhất
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetListAssetCodeAsync();

        /// <summary>
        /// filter, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch);

        /// <summary>
        /// lấy dữ liệu tài sản đề xuất file excel
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<FixedAssetExcel>> GetFixedAssetsExcelAsync();
    }
}
