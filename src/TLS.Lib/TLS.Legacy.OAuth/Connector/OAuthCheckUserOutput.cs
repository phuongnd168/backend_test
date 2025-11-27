using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Legacy.OAuth.Connector
{
    public class OAuthCheckUserOutput
    {
        /// <summary>
        /// 0: Success
        /// 1: Password khong dung
        /// 2: UserName da ton tai
        /// 3: Email da ton tai
        /// 4: PhoneNumber da ton tai
        /// 9: Loi khac
        /// </summary>
        public int Status { get; set; }
    }
}
