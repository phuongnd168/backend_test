using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.User
{
    public class UserItem
    {
        public int Id { get; set; }

        public string UserId {  get; set; }

        public string FullName { get; set; }

        public string Mobile {  get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Avatar { get; set; }

        public int Status {  get; set; }

        public int Type {  get; set; }
    }
}
