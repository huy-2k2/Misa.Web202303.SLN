using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.Common.Const;

namespace Misa.Web202303.QLTS.BL.AuthService
{
    public class AuthDto
    {
        /// <summary>
        /// email
        /// </summary>
        [Required, Name(FieldName.Email)]
        public string email { get; set; }

        /// <summary>
        /// mật khẩu
        /// </summary>
        [Required, Name(FieldName.Password)]
        public string password { get; set; }
    }
}
