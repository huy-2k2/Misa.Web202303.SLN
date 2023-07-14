using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.Common.Const;

/// <summary>
/// chứa tên các filed name để trả về exception cho client, sử dụng kết hợp với string.Format()
/// created by: 08/06/2023 (nguyễn quốc huy)
/// </summary>
public static class FieldName
{
    /// <summary>
    /// mã tài sản
    /// </summary>
    public const string FixedAssetCode = "mã tài sản";

    /// <summary>
    /// tên tài sản
    /// </summary>
    public const string FixedAssetName = "tên tài sản";

    /// <summary>
    /// ngày mua
    /// </summary>
    public const string PurchaseDate = "ngày mua";

    /// <summary>
    /// ngày sử dụng
    /// </summary>
    public const string UseDate = "ngày sử dụng";

    /// <summary>
    /// nguyên giá
    /// </summary>
    public const string Cost = "nguyên giá";

    /// <summary>
    /// số lượng
    /// </summary>
    public const string Quantity = "số lượng";

    /// <summary>
    /// tỷ lệ hao mòn
    /// </summary>
    public const string DepreciationRate = "tỷ lệ hao mòn";

    /// <summary>
    /// giá trị hao mòn năm
    /// </summary>
    public const string DepreciationAnnual = "giá trị hao mòn năm";

    /// <summary>
    /// năm theo dõi
    /// </summary>
    public const string TrackedYear = "năm theo dõi";

    /// <summary>
    /// số năm sử dụng
    /// </summary>
    public const string LifeTime = "số năm sử dụng";

    /// <summary>
    /// mã loại tài sản
    /// </summary>
    public const string FixedAssetCategoryCode = "mã loại tài sản";

    /// <summary>
    /// tên loại tài sản
    /// </summary>
    public const string FixedAssetCategoryName = "tên loại tài sản";

    /// <summary>
    /// mã bộ phận sử dụng
    /// </summary>
    public const string DepartmentCode = "mã bộ phận sử dụng";

    /// <summary>
    /// tên bộ phận sử dụng
    /// </summary>
    public const string DepartmentName = "tên bộ phận sử dụng";

    /// <summary>
    /// trang hiện tại
    /// </summary>
    public const string CurrentPage = "trang hiện tại";

    /// <summary>
    /// kích thước trang
    /// </summary>
    public const string PageSize = "kích thước trang";

    /// <summary>
    /// loại tài sản
    /// </summary>
    public const string FixedAssetCategory = "loại tài sản";

    /// <summary>
    /// phòng ban
    /// </summary>
    public const string Department = "phòng ban";

    /// <summary>
    /// mã
    /// </summary>
    public const string CommonCode = "mã";

    /// <summary>
    /// email
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// mật khẩu
    /// </summary>
    public const string Password = "mật khẩu";

    public const string Budget = "nguồn ngân sách";

    public const string FixedAsset = "tài sản";

    public const string License = "Chứng từ";

    public const string LicenseCode = "Mã chứng từ";

    public const string BudgetValue = "Giá trị ngân sách";
}
