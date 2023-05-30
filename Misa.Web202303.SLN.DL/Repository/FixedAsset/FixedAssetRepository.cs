using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.SLN.Common.Const;
using Misa.Web202303.SLN.DL.Entity;
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
    /// thực thi các phương thức của IFixedAssetRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FixedAssetRepository : BaseRepository<FixedAssetEntity>, IFixedAssetRepository
    {

        /// <summary>
        /// hàm khởi tạo
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="configuration"></param>
        public FixedAssetRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Để đếm số bản ghi thực sự tồn tại trong danh sách các id, dùng để kiểm tra khi xóa nhiều bản ghi cùng lúc
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        public async Task<int> CountFixedAssetInListIdAsync(string listFixedAssetId)
        {
            var connection = await GetOpenConnectionAsync();

            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_ids", listFixedAssetId);
            var sql = ProcedureName.COUNT_FIXED_ASSET_BY_ID;
            var result = await connection.QueryFirstAsync<int>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            await connection.CloseAsync();
            return result;
        }


        /// <summary>
        /// xóa nhiều tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string listFixedAssetId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.DELETE_FIXED_ASSETS;
            //thêm params
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_ids", listFixedAssetId);


            var result = await connection.ExecuteAsync(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            await connection.CloseAsync();

            return true;
        }

        /// <summary>
        /// lọc, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public async Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            var connection = await GetOpenConnectionAsync();
            // thêm các param
            var dynamicParams = new DynamicParameters();
            var sql = ProcedureName.FILTER_FIXED_ASSETS;
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            // tham số nào là null thì truyền vào procedure là chuỗi rỗng
            dynamicParams.Add("department_id", departmentId == null? "" : departmentId);
            dynamicParams.Add("fixed_asset_category_id", fixedAssetCategoryId == null? "" : fixedAssetCategoryId);
            dynamicParams.Add("text_search", textSearch ?? "");
            // thêm param output để lấy tổng tài sản, tổng số lượng, tổng nguyên giá
            dynamicParams.Add("total_asset", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_quantity", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_cost", dbType: DbType.Double, direction: ParameterDirection.Output);
            
            var listFixedAsset = await connection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            // lấy tổng tài sản, tổng số lượng, tổng  nguyên giá
            var totalAsset = dynamicParams.Get<int>("total_asset");
            var totalQuantity = dynamicParams.Get<int?>("total_quantity");
            var totalCost = dynamicParams.Get<double?>("total_cost");

            await connection.CloseAsync();

            return new FilterListFixedAsset() { List_fixed_asset = listFixedAsset, Total_asset = totalAsset, Total_cost=totalCost ?? 0, Total_quantity=totalQuantity ?? 0};

        }

        /// <summary>
        /// lấy dữ liệu tài sản đề xuất file excel
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FixedAssetExcel>> GetFixedAssetsExcelAsync()
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_MODEL_FIXED_ASSETS;
            var dynamicParams = new DynamicParameters();
            var excelFixedAssets = await connection.QueryAsync<FixedAssetExcel>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            await connection.CloseAsync();
            return excelFixedAssets; 
        }

        /// <summary>
        /// lấy ra các mã tài sản có cùng tiền tố với mã tài sản được thêm hoạc sủa gần nhất
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetListAssetCodeAsync()
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_NEWEST_FIXED_ASSET_CODES;
            // thêm các param
            var dynamicParams = new DynamicParameters();
            // thêm param output để lấy ra tiền tố của mã tài sản
            dynamicParams.Add("prefix_asset_code", dbType: DbType.String, direction: ParameterDirection.Output);
            var listFixedAsset = await connection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            // lấy ra các mã tài sản ở dạng list string
            var result =  listFixedAsset.Select((fixedAsset) =>  fixedAsset.Fixed_asset_code).ToList();
            // lấy ra tiền tố
            var preFix = dynamicParams.Get<string>("prefix_asset_code");
            // thêm tiền tố vào cuối danh sách mã tài sản
            result.Add(preFix);
            await connection.CloseAsync();

            return result;
        }


        /// <summary>
        /// lấy ra tên của table trong csdl ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        protected override string GetTableName()
        {
            return "fixed_asset";
        }
    }
}
