using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.Common.Emum
{
    /// <summary>
    /// chứa các mã lỗi cụ thể
    /// Created by: NQ Huy(10/05/2023)
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// lỗi server
        /// </summary>
        Exception = 0,

        /// <summary>
        /// mã code trùng
        /// </summary>
        DuplicateCode = 1,

        /// <summary>
        /// lỗi validate dữ liệu
        /// </summary>
        DataValidate = 2,

        /// <summary>
        /// lỗi validate nghiệp vụ
        /// </summary>
        BusinessValidate = 3,

        /// <summary>
        /// lỗi không tìm thấy dữ liệu
        /// </summary>
        NotFound = 4,

        /// <summary>
        /// dữ liệu không hợp lệ
        /// </summary>
        InvalidData = 5,

        /// <summary>
        /// lỗi file
        /// </summary>
        FileInvalid = 6,

        /// <summary>
        /// thông tin đăng nhập không hợp lệ
        /// </summary>
        CredentialNotValid = 7,

        /// <summary>
        /// token xác thực không hợp lệ
        /// </summary>
        InvalidToken = 8,

        DeleteDetail = 9,

        DeleteDetailMulti = 10

    }
}
