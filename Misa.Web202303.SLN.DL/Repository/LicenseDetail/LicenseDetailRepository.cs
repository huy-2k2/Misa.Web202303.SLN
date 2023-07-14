using Dapper;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
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
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();
            var sql = $"SELECT * FROM {tableName} WHERE license_id = @licenseId AND FIND_IN_SET(license_detail_id, @listId) != 0";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@listId", listId);
            dynamicParams.Add("@licenseId", licenseId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<LicenseDetailEntity>(sql, dynamicParams, transaction);

            return result;
        }

        public async Task<IEnumerable<LicenseDetailEntity>> GetListFAExistedAsync(string listFAId)
        {
            var tableName = GetTableName();
            var connection = await GetOpenConnectionAsync();
            var sql = $"SELECT * FROM {tableName} WHERE FIND_IN_SET(fixed_asset_id, @listFAId) != 0";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@listFAId", listFAId);

            var transaction = await _unitOfWork.GetTransactionAsync();

            var result = await connection.QueryAsync<LicenseDetailEntity>(sql, dynamicParams, transaction);

            return result;
        }

        public override string GetTableName()
        {
            return "license_detail";
        }
    }
}
