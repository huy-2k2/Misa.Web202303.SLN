﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Misa.Web202303.SLN.BL.Service.FixedAsset
{
    /// <summary>
    /// interface định nghĩa thêm các phương thức của FixedAssetService, ngoài các phương thức của IBaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public interface IFixedAssetService : IBaseService<FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>
    {
        /// <summary>
        /// xóa nhiều tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<Guid> listFixedAssetId);


        /// <summary>
        /// tự động sinh mã tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<string> GetRecommendFixedAssetCodeAsync();

        /// <summary>
        /// filter, search, phân trang
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        Task<object> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch);

        /// <summary>
        /// lấy dữ liệu tài sản đề xuất file excel
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetFixedAssetsExcelAsync();
    }
}
