using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Const;

/// <summary>
/// chứa tên các filed name để trả về exception cho client, sử dụng kết hợp với string.Format()
/// created by: 08/06/2023 (nguyễn quốc huy)
/// </summary>
public static class FieldName
{
    public const string FixedAssetCode = "mã tài sản";

    public const string FixedAssetName = "tên tài sản";

    public const string PurchaseDate = "ngày mua";

    public const string UseDate = "ngày sử dụng";

    public const string Cost = "nguyên giá";

    public const string Quantity = "số lượng";

    public const string DepreciationRate = "tỷ lệ hao mòn";

    public const string DepreciationAnnual = "giá trị hao mòn năm";

    public const string TrackedYear = "năm theo dõi";

    public const string LifeTime = "số năm sử dụng";

    public const string FixedAssetCategoryCode = "mã loại tài sản";

    public const string FixedAssetCategoryName = "tên loại tài sản";

    public const string DepartmentCode = "mã bộ phận sử dụng";

    public const string DepartmentName = "tên bộ phận sử dụng";

    public const string CurrentPage = "trang hiện tại";

    public const string PageSize = "kích thước trang";

    public const string FixedAssetCategory = "loại tài sản";

    public const string Department = "phòng ban";

    public const string CommonCode = "mã";
}
