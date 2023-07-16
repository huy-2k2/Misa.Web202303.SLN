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

        // lấy ra danh sách license detail có id nằm trong list cho trước và thuộc 1 chứng từ cho trước
        public const string GET_LIST_LICENSE_DETAIL_EXISTED_OF_LICENSE = "proc_get_list_license_detail_existed_of_license";

        // lấy ra danh sách tài sản của 1 chứng từ
        public const string GET_LIST_FIXED_ASSET_BY_LICENSE = "proc_get_list_fixed_asset_by_license";

        // lấy ra danh sách  budget_detail có id nằm trong list cho trước và thuộc 1 chứng từ cho trước
        public const string GET_LIST_BUDGET_DETAIL_EXISTED_OF_LICENSE = "proc_get_list_budget_detail_existed_of_license";

        // lấy ra các budget_detail có fixed_asset_id và budget_id nằm trong list cho trước
        public const string GET_LIST_BUDGET_DETAIL_EXISTED_BY_BF = "proc_get_list_budget_detail_existed_by_bf";

        // Lấy ra các license detail có fixed_asset_id nằm trong list cho trước
        public const string GET_LIST_LICENSE_dETAIL_EXISTED_BY_FIXED_ASSET = "proc_get_list_license_detail_existed_by_fixed_asset";

        // xóa chi tiết nguồn ngân sách theo chi tiết chứng từ
        public const string DELETE_BUDGET_DETAIL_BY_LIST_LICENSE_DETAIL = "proc_delete_budget_detail_by_list_license_detail";

        // xóa chi tiết nguồn ngân sách theo list chứng từ
        public const string DELETE_BUDGET_DETAIL_BY_LIST_LICENSE = "proc_delete_budget_detail_by_list_license";



    }
}
