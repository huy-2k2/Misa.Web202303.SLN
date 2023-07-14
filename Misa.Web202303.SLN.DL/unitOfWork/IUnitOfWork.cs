using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.unitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// lấy ra connection cho 1 request
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns>connection</returns>
        DbConnection GetDbConnection();

        /// <summary>
        /// lấy ra transaction cho 1 request
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns>transaction</returns>
        DbTransaction GetTransaction();

        /// <summary>
        /// rollback khi xảy ra exception
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns></returns>
        void Rollback();

        /// <summary>
        /// commit transaction
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns></returns>
        void Commit();

        /// <summary>
        /// lấy ra connection cho 1 request
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns>connection</returns>
        Task<DbConnection> GetDbConnectionAsync();

        /// <summary>
        /// lấy ra transaction cho 1 request
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns>transaction</returns>
        Task<DbTransaction> GetTransactionAsync();

        /// <summary>
        /// rollback khi xảy ra exception
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();

        /// <summary>
        /// commit transaction
        /// created by: NQ Huy (07/04/2023)
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// đóng kết nối
        /// created by: NQ Huy(07/04/2023)
        /// </summary>
        /// <returns></returns>
        void SaveChange();

        /// <summary>
        /// đóng kết nối
        /// created by: NQ Huy(07/04/2023)
        /// </summary>
        /// <returns></returns>
        Task SaveChangeAsync();

    }
}
