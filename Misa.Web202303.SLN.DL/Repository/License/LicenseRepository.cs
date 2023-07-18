using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;
using LicenseModel = Misa.Web202303.QLTS.DL.Model.License;
using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;
using FixedAssetModel = Misa.Web202303.QLTS.DL.Model.FixedAsset;
using Misa.Web202303.QLTS.DL.Filter;
using Misa.Web202303.QLTS.Common.Const;
using System.Data;
using Misa.Web202303.QLTS.DL.unitOfWork;

namespace Misa.Web202303.QLTS.DL.Repository.License
{
    public class LicenseRepository : BaseRepository<LicenseEntity>, ILicenseRepository
    {
        /// <summary>
        /// phương thức khởi tạo
        /// created by: NQ Huy(28/06/2023)
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        public LicenseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// lấy danh sách license Model (license và tài sản liên quan)
        /// created by: NQ Huy(28/06/2023)
        /// </summary>
        /// <param name="pageSize">kích thước trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa truy vấn</param>
        /// <returns>danh sách license Model</returns>
        public async Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string? textSearch)
        {
            // tạo connection, add các tham số
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_FILTER_MODEL_LICENSES;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            dynamicParams.Add("text_search", textSearch ?? "");

            // lấy tham số kiểu out
            dynamicParams.Add("total_license", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_cost", dbType: DbType.Double, direction: ParameterDirection.Output);

            var transaction = await _unitOfWork.GetTransactionAsync();

            // gọi truy vấn
            var listLicense = await connection.QueryAsync<LicenseModel>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);


            // lấy ra tham số kiểu out
            var totalLicense = dynamicParams.Get<int>("total_license");
            var totalCost = dynamicParams.Get<double?>("total_cost");


            // trả về kết quả
            return new FilterLicenseModel()
            {
                list_license = listLicense,
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
