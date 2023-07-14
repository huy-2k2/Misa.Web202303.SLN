using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.DL.Filter;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FixedAssetEntity = Misa.Web202303.QLTS.DL.Entity.FixedAsset;
using FixedAssetModel = Misa.Web202303.QLTS.DL.Model.FixedAsset;

namespace Misa.Web202303.QLTS.DL.Repository.FixedAsset
{
    /// <summary>
    /// thực thi các phương thức của IFixedAssetRepository, kế thừa các phương thức có sẵn của BaseRepository
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public class FixedAssetRepository : BaseRepository<FixedAssetEntity>, IFixedAssetRepository
    {
        #region
        public FixedAssetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #endregion

        #region
        /// <summary>
        /// Filter, search, phân trang tài sản
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="departmentId">mã phòng ban</param>
        /// <param name="fixedAssetCategoryId">mã loại tài sản</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns>danh sách tài sản thỏa mãn yêu cầu Filter, phân trang</returns>
        public async Task<FilterListFixedAsset> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            var connection = await GetOpenConnectionAsync();
            // thêm các param
            var dynamicParams = new DynamicParameters();
            var sql = ProcedureName.FILTER_FIXED_ASSETS;
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            // tham số nào là null thì truyền vào procedure là chuỗi rỗng
            dynamicParams.Add("department_id", departmentId == null ? "" : departmentId);
            dynamicParams.Add("fixed_asset_category_id", fixedAssetCategoryId == null ? "" : fixedAssetCategoryId);
            dynamicParams.Add("text_search", textSearch ?? "");
            // thêm param output để lấy tổng tài sản, tổng số lượng, tổng nguyên giá
            dynamicParams.Add("total_asset", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_quantity", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dynamicParams.Add("total_cost", dbType: DbType.Double, direction: ParameterDirection.Output);

            var transaction = await _unitOfWork.GetTransactionAsync();


            var listFixedAsset = await connection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, transaction: transaction, commandType: CommandType.StoredProcedure);
            // lấy tổng tài sản, tổng số lượng, tổng  nguyên giá
            var totalAsset = dynamicParams.Get<int>("total_asset");
            var totalQuantity = dynamicParams.Get<int?>("total_quantity");
            var totalCost = dynamicParams.Get<double?>("total_cost");


            return new FilterListFixedAsset() { list_fixed_asset = listFixedAsset, total_asset = totalAsset, total_cost = totalCost ?? 0, total_quantity = totalQuantity ?? 0 };

        }

        /// <summary>
        /// lấy ra các tài sản chưa có chứng từ và chưa được chọn
        /// </summary>
        /// <param name="listIdSelected">danh sách id đã được chọn</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <param name="pageSize">kích thước của 1 page</param>
        /// <param name="currentPage">page hiện tại</param>
        /// <returns>danh sách tài sản</returns>
        public async Task<FilterListFixedAsset> GetFilterNotHasLicenseAsync(int pageSize, int currentPage, string listIdSelected, string? textSearch, Guid? license_id)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_FILTER_FIXED_ASSET_NO_LICENSE;
            var dynamicParams = new DynamicParameters();
            // add param
            dynamicParams.Add("page_size", pageSize);
            dynamicParams.Add("current_page", currentPage);
            dynamicParams.Add("list_id_selected", listIdSelected);
            dynamicParams.Add("text_search", textSearch);
            dynamicParams.Add("license_id", license_id == null? "": license_id);
            dynamicParams.Add("total_asset", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var listFixedAsset = await connection.QueryAsync<FixedAssetEntity>(sql, dynamicParams, transaction: transaction, commandType: CommandType.StoredProcedure);
            // lấy ra param kiểu out
            var totalAsset = dynamicParams.Get<int>("total_asset");
            // trả về kết quả
            return new FilterListFixedAsset()
            {
                list_fixed_asset = listFixedAsset,
                total_asset = totalAsset,
            };
        }

        /// <summary>
        /// lấy danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseId">id chứng từ</param>
        /// <returns>danh sách tài sản</returns>
        public async Task<IEnumerable<FixedAssetModel>> GetListByLicenseId(Guid licenseId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = "SELECT fa.*, license_detail_id FROM fixed_asset fa JOIN license_detail ld ON ld.fixed_asset_id = fa.fixed_asset_id WHERE ld.license_id = @license_id";
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("license_id", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();


            var listFixedAsset = await connection.QueryAsync<FixedAssetModel>(sql, dynamicParams, transaction: transaction);

            return listFixedAsset;
        }




        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>tên của table fixed asset</returns>
        public override string GetTableName()
        {
            return "fixed_asset";
        }
        #endregion
    }
}
