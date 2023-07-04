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
        DbConnection GetDbConnection();

        DbTransaction GetTransaction();

        void Rollback();

        void Commit();
    }
}
