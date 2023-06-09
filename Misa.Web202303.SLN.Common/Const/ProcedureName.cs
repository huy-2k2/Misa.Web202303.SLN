using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Const
{
    public static class ProcedureName
    {
        // filter tài sản
        public const string FILTER_FIXED_ASSETS = "proc_filter_assets";

        // lấy ra các mã tài sản có cùng tiền tố với tài sản mới nhất
        public const string GET_MAX_FIXED_ASSET_CODE = "proc_get_max_fixed_asset_code";
    }
}
