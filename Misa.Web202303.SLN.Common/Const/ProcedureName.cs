using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Const
{
    public static class ProcedureName
    {
        
        // tổng số tài sản tồn tại trong list tài sản
        public const string COUNT_FIXED_ASSET_BY_ID = "proc_count_fixed_asset_by_ids";

        // xóa nhiều tài sản cùng lúc
        public const string DELETE_FIXED_ASSETS = "proc_delete_assets";

        // filter tài sản
        public const string FILTER_FIXED_ASSETS = "proc_filter_assets";

        // lấy ra các mã tài sản có cùng tiền tố với tài sản mới nhất
        public const string GET_NEWEST_FIXED_ASSET_CODES = "proc_get_newest_fixed_asset_codes";

        // lấy ra model tài sản để export file excel
        public const string GET_MODEL_FIXED_ASSETS = "proc_get_excel_fixed_assets";


    }
}
