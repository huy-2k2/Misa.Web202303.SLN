using Dapper;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;

namespace Misa.Web202303.QLTS.DL.Repository.BudgetDetail
{
    public class BudgetDetailRepository : BaseRepository<BudgetDetailEntity>, IBudgetDetailRepository
    {
        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(10/07/2023)
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        public BudgetDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        /// xóa các budget_detail theo danh sách license_detail_id
        /// </summary>
        /// <param name="listLicenseDetailId">danh sách license_detail_id, các id cách nhau bởi dấu ","</param>
        /// <returns></returns>
        public async Task DeleteByListLicenseDetailId(string listLicenseDetailId)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.DELETE_BUDGET_DETAIL_BY_LIST_LICENSE_DETAIL;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listLicenseDetailId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// xóa các budget_detail theo danh sách license_id
        /// </summary>
        /// <param name="listLicenseId">danh sách license_id, các id nối cách nhau bởi dấu ","</param>
        /// <returns></returns>
        public async Task DeleteListByListLicenseId(string listLicenseId)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.DELETE_BUDGET_DETAIL_BY_LIST_LICENSE;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listLicenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            await connection.ExecuteAsync(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// lấy ra danh sách budget_detail có budget_id và fixed_asset_id nối lại thì nằm trong 1 list id cho trước
        /// </summary>
        /// <param name="listBFId">danh sách id của fixed_asset và budget nối cách nhau bởi dấu ".", giữa nhiều cặp id cách nhau bởi dấu "," </param>
        /// <returns>Danh sách BudgetDetailEntity</returns>
        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedBFAsync(string listBFId)
        {
            var connection = await GetOpenConnectionAsync();

            var sql = ProcedureName.GET_LIST_BUDGET_DETAIL_EXISTED_BY_BF;

            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_bf_id", listBFId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        /// <summary>
        /// lấy ra các budget_detail có id nằm trong list_id cho trước, và thuộc một chứng từ có id cho trước
        /// </summary>
        /// <param name="licenseId">id của chứng từ</param>
        /// <param name="listId">danh sách budget_detail_id nối cách nhau bởi dấu ","</param>
        /// <returns>danh sách BudgetDetailEntity thỏa mã điều kiện</returns>
        public async Task<IEnumerable<BudgetDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_LIST_BUDGET_DETAIL_EXISTED_OF_LICENSE;
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("list_id", listId);
            dynamicParams.Add("license_id", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<BudgetDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        #region
        /// <summary>
        /// lấy ra tên cụ thể của table ứng với repository
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns>tên của table budget_detail</returns>
        public override string GetTableName()
        {
            return "budget_detail";
        }
        #endregion
    }
}
