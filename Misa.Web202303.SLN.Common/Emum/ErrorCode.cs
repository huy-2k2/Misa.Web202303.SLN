using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.Common.Emum
{
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
        FileInvalid = 6

    }
}
