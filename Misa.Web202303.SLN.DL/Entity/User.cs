using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.DL.Entity
{
    public class User
    {
        /// <summary>
        /// id người dùng
        /// </summary>
        public Guid user_id { get; set; }

        /// <summary>
        /// tên người dùng
        /// </summary>
        public string name { get; set;}

        /// <summary>
        /// email người dùng
        /// </summary>
        public string email { get; set;}

        /// <summary>
        /// mật khẩu người dùng
        /// </summary>
        public string password { get; set;} 
    }
}
