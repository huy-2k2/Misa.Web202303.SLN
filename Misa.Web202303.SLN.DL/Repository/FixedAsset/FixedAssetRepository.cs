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
        /// lấy mã tài sản có cùng tiền tố với mã tài sản được thêm hoạc sửa gần nhất và có hậu tố lớn nhất
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetMaxAssetCodeAsync()
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_MAX_FIXED_ASSET_CODE;
            // thêm các param
            var dynamicParams = new DynamicParameters();
            // thêm param output để lấy ra tiền tố của mã tài sản, và tài sản có hậu tố lớn nhất
            dynamicParams.Add("prefix_asset_code", dbType: DbType.String, direction: ParameterDirection.Output);
            dynamicParams.Add("max_asset_code", dbType: DbType.String, direction: ParameterDirection.Output);
            await connection.ExecuteAsync(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            
            var result =  new List<string>();
            // lấy ra tiền tố
            var preFix = dynamicParams.Get<string>("prefix_asset_code");
            // lấy ra mã tài sản lớn nhất
            var maxAssetCode = dynamicParams.Get<string>("max_asset_code");

            result.Add(preFix);
            result.Add(maxAssetCode);

            await connection.CloseAsync();

            return result;
        }

        /// <summary>
        /// lấy ra tên của table trong csdl ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "Fixed_asset";
        }
    }
}
