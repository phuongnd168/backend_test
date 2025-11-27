using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Legacy.OAuth.Connector
{
    public class OAuthCreateUserOutput
    {
        /// <summary>
        /// 0: Success
        /// 1: Loi - Ví dụ đã tồn tại UserName or Email or PhoneNumber, Password ko đủ 6 ký tự
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// User ID vừa tạo
        /// </summary>
        public int UserId { get; set; }
    }
}
