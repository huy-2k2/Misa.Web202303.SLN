using Dapper;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseDetailEntity = Misa.Web202303.QLTS.DL.Entity.LicenseDetail;

namespace Misa.Web202303.QLTS.DL.Repository.LicenseDetail
{
    public class LicenseDetailRepository : BaseRepository<LicenseDetailEntity>, ILicenseDetailRepository
    {
        public LicenseDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<LicenseDetailEntity>> GetListExistedOfLicenseAsync(Guid licenseId, string listId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_LIST_LICENSE_DETAIL_EXISTED_OF_LICENSE;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("list_id", listId);
            dynamicParams.Add("license_id", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<LicenseDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<IEnumerable<LicenseDetailEntity>> GetListFAExistedAsync(string listFAId)
        {
            var connection = await GetOpenConnectionAsync();
            var sql = ProcedureName.GET_LIST_LICENSE_dETAIL_EXISTED_BY_FIXED_ASSET;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("list_fixed_asset_id", listFAId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<LicenseDetailEntity>(sql, dynamicParams, transaction, commandType: CommandType.StoredProcedure);

            return result;
        }

        public override string GetTableName()
        {
            return "license_detail";
        }
    }
}
