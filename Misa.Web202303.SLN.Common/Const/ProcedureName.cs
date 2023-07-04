using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.Common.Const
{
    /// <summary>
    /// tên của procedure
    /// Created by: NQ Huy(10/05/2023)
    /// </summary>
    /// 
    public static class ProcedureName
    {
        // filter tài sản
        public const string FILTER_FIXED_ASSETS = "proc_filter_assets";

        // lấy ra các mã tài sản có cùng tiền tố với tài sản mới nhất
        public const string GET_MAX_FIXED_ASSET_CODE = "proc_get_max_code";

        //filter licence
        public const string GET_FILTER_MODEL_LICENSES = "proc_filter_license_model";

        // phân trang, filter các tài sản chưa có chứng từ
        public const string GET_FILTER_FIXED_ASSET_NO_LICENSE = "proc_filter_assets_no_license";

        // lấy ngân sách theo id tài sản
        public const string GET_LIST_BUDGET_MODEL = "proc_get_list_budget_model";
    }
}
