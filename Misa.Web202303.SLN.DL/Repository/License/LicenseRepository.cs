using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;
using LicenseModel = Misa.Web202303.QLTS.DL.model.License;
using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;
using Misa.Web202303.QLTS.DL.filter;
using Misa.Web202303.QLTS.Common.Const;
using System.Data;

namespace Misa.Web202303.QLTS.DL.Repository.License
{
    public class LicenseRepository : BaseRepository<LicenseEntity>, ILicenseRepository
    {
        /// <summary>
        /// hàm khởi tạo
        /// created by:NQ Huy(29/06/2023)
        /// </summary>
        /// <param name="configuration">configuration</param>
        public LicenseRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// lấy danh sách license model (license và tài sản liên quan)
        /// created by: NQ Huy(28/06/2023)
        /// </summary>
        /// <param name="pageSize">kích thước trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa truy vấn</param>
        /// <returns>danh sách license model</returns>
        public async Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string? textSearch)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_FILTER_MODEL_LICENSES;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            dynamicParams.Add("text_search", textSearch ?? "");


            dynamicParams.Add("total_license", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_cost", dbType: DbType.Double, direction: ParameterDirection.Output);

            /// xử lý câu truy vấn có dùng join để tạo thành model
            var licenses = await connection.QueryAsync<LicenseModel, FixedAssetEntity, LicenseModel>(sql: sql, param: dynamicParams, commandType: CommandType.StoredProcedure, splitOn: "fixed_asset_id", map: (licenseModel, fixedAsset) =>
            {
                if (licenseModel.fixed_assets == null)
                {
                    licenseModel.fixed_assets = new List<FixedAssetEntity>();
                }

                licenseModel.fixed_assets.Add(fixedAsset);
                return licenseModel;
            });

            // nhóm các license có trùng id
            var listLicenseModel = licenses.GroupBy(l => l.license_id).Select(g =>
            {
                var groupedModel = g.First();
                var listAsset = g.Select(model => model.fixed_assets.First()).ToList();
                if (listAsset.First() != null)
                    groupedModel.fixed_assets = listAsset;
                else
                    groupedModel.fixed_assets = new List<FixedAssetEntity>();
                return groupedModel;
            });

            // lấy ra tham số kiểu out
            var totalLicense = dynamicParams.Get<int>("total_license");
            var totalCost = dynamicParams.Get<double?>("total_cost");

            await connection.CloseAsync();

            // trả về kết quả
            return new FilterLicenseModel()
            {
                list_license_model = listLicenseModel,
                total_cost = totalCost ?? 0,
                total_license = totalLicense,
            };
        }

        /// <summary>
        /// hàm lấy ra tên bảng tương ứng trong db
        /// created by:NQ Huy (29/06/2023)
        /// </summary>
        /// <returns>tên bảng</returns>
        public override string GetTableName()
        {
            return "license";
        }
    }
}
