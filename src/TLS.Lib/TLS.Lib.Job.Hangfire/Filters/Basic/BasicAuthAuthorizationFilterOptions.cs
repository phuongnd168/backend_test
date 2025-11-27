using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Job.Hangfire.Filters.Basic
{
    /// <summary>
    /// Represents options for Hangfire basic authentication
    /// </summary>
    public class BasicAuthAuthorizationFilterOptions
    {
        public BasicAuthAuthorizationFilterOptions()
        {
            SslRedirect = false;
            RequireSsl = false;
            LoginCaseSensitive = false;
            Users = new List<BasicAuthAuthorizationUser>();
        }

        public void AddUser(string login, byte[] password, bool clear = false)
        {
            if (Users == null)
            {
                Users = new List<BasicAuthAuthorizationUser>();
            }
            var users = Users.ToList();
            var user = new BasicAuthAuthorizationUser
            {
                Login = login
            };
            if (clear)
            {
                user.PasswordClear = Encoding.UTF8.GetString(password);
            }
            else
            {
                user.Password = password;
            }    
            users.Add(user);
            Users = users;
        }

        /// <summary>
        /// Redirects all non-SSL requests to SSL URL
        /// </summary>
        public bool SslRedirect { get; set; }

        /// <summary>
        /// Requires SSL connection to access Hangfire dahsboard. It's strongly recommended to use SSL when you're using basic authentication.
        /// </summary>
        public bool RequireSsl { get; set; }

        /// <summary>
        /// Whether or not login checking is case sensitive.
        /// </summary>
        public bool LoginCaseSensitive { get; set; }

        /// <summary>
        /// Represents users list to access Hangfire dashboard.
        /// </summary>
        public IEnumerable<BasicAuthAuthorizationUser> Users { get; set; }
    }
}
