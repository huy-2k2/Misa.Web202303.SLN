﻿using Dapper;
using Microsoft.Extensions.Configuration;
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
        /// kiểm tra trùng mã khi thêm và sửa
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="fixedAssetCode"></param>
        /// <param name="fixedAssetId"></param>
        /// <returns></returns>
        public async Task<bool> CheckAssetCodeExistedAsync(string fixedAssetCode, Guid? fixedAssetId)
        {
            var connection = await GetOpenConnectionAsync();
            // thêm các param
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_ids", fixedAssetCode);

            // nếu không truyền fixedAssetId thì truyền param là chuỗi rỗng
            dynamicParams.Add("fixed_asset_id", fixedAssetId == null ? "" : fixedAssetId);

            // trả về độ dài mã tài sản tìm được, nếu không thì tra về 0
            var result = await connection.QueryFirstAsync<int>("prop_is_fixed_asset_code_existed", dynamicParams, commandType: CommandType.StoredProcedure);

            return result != 0;
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
            //thêm params
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("fixed_asset_ids", listFixedAssetId);


            var result = await connection.ExecuteAsync("proc_delete_assets", dynamicParams, commandType: CommandType.StoredProcedure);

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
            var sql = "proc_filter_assets";
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            // tham số nào là null thì truyền vào procedure là chuỗi rỗng
            dynamicParams.Add("department_id", departmentId == null? "" : departmentId);
            dynamicParams.Add("fixed_asset_category_id", fixedAssetCategoryId == null? "" : fixedAssetCategoryId);
            dynamicParams.Add("text_search", textSearch ?? "");
            // thêm param output để lấy tổng tài sản
            dynamicParams.Add("total_asset", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var listFixedAsset = await connection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            // lấy tổng tài sản
            var totalAsset = dynamicParams.Get<int>("total_asset");

            return new FilterListFixedAsset() { List_fixed_asset = listFixedAsset, Total_asset = totalAsset};

        }

        /// <summary>
        /// lấy ra các mã tài sản có cùng tiền tố với mã tài sản được thêm hoạc sủa gần nhất
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetListAssetCodeAsync()
        {
            var connnection = await GetOpenConnectionAsync();
            var sql = "proc_get_newest_fixed_asset_codes";
            // thêm các param
            var dynamicParams = new DynamicParameters();
            // thêm param output để lấy ra tiền tố của mã tài sản
            dynamicParams.Add("prefix_asset_code", dbType: DbType.String, direction: ParameterDirection.Output);
            var listFixedAsset = await connnection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, commandType: CommandType.StoredProcedure);
            // lấy ra các mã tài sản ở dạng list string
            var result =  listFixedAsset.Select((fixedAsset) =>  fixedAsset.Fixed_asset_code).ToList();
            // lấy ra tiền tố
            var preFix = dynamicParams.Get<string>("prefix_asset_code");
            // thêm tiền tố vào cuối danh sách mã tài sản
            result.Add(preFix);
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
