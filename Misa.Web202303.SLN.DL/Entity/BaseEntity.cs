using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.DL.Entity
{
    /// <summary>
    /// lớp cơ sở
    /// Created by: NQ Huy(20/05/2023)
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// thời gian tạo
        /// </summary>
        public DateTime? Created_date { get; set; }

        /// <summary>
        /// thời gian sửa
        /// </summary>
        public DateTime? Modified_date { get; set; }

        /// <summary>
        /// tên người tạo
        /// </summary>
        public string? Created_by { get; set; }

        /// <summary>
        /// tên người sửa
        /// </summary>
        public string? Modified_by { get; set; }
    }
}
