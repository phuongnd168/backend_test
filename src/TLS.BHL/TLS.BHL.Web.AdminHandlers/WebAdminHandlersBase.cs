using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Web.Handlers;

namespace TLS.BHL.Web.AdminHandlers
{
    public class WebAdminHandlersBase<T> : RequestHandlerBase<T> where T : class
    {
        protected IConfiguration Configuration => ServiceProvider.GetRequiredService<IConfiguration>();
        public WebAdminHandlersBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        ///// <summary>
        ///// Current logged in user ID
        ///// </summary>
        //protected int? UserID
        //{
        //    get
        //    {
        //        return User.GetIdentityUserId();
        //    }
        //}

        ///// <summary>
        ///// Current logged in user name
        ///// </summary>
        //protected string UserName
        //{
        //    get
        //    {
        //        return User.GetIdentityUserName();
        //    }
        //}

        ///// <summary>
        ///// Current logged in user email
        ///// </summary>
        //protected string UserEmail
        //{
        //    get
        //    {
        //        return User.GetIdentityUserEmail();
        //    }
        //}

        ///// <summary>
        ///// Current logged in user name or email
        ///// </summary>
        //protected string UserNameOrEmail
        //{
        //    get
        //    {
        //        if (UserID > 0)
        //        {
        //            return UserName;
        //        }
        //        else
        //        {
        //            return UserEmail;
        //        }
        //    }
        //}

        /// <summary>
        /// Trace identifier of current request
        /// </summary>
        protected string RequestID
        {
            get
            {
                return HttpContext.TraceIdentifier;
            }
        }

        /// <summary>
        /// Last 8 characters of RequestID
        /// </summary>
        protected string RequestIDShort
        {
            get
            {
                return RequestID.Substring(RequestID.Length - 8);
            }
        }

        //private AuthenticateNhanVien _nhanVien;
        //protected AuthenticateNhanVien NhanVien
        //{
        //    get
        //    {
        //        if (User.Identity.IsAuthenticated && _nhanVien == null)
        //        {
        //            _nhanVien = User.GetIdentityUser((userNameOrEmail, tokenKey) => {
        //                return HttpContext.RequestServices.GetService<INhanVienService>().GetOneAuthenticateNhanVien(userNameOrEmail, tokenKey).GetAwaiter().GetResult();
        //            });
        //        }
        //        return _nhanVien;
        //    }
        //}

        //private DMCongTyEntity _congTy;
        //protected DMCongTyEntity CongTy
        //{
        //    get
        //    {
        //        if (User.Identity.IsAuthenticated && _congTy == null)
        //        {
        //            if (NhanVien != null)
        //            {
        //                _congTy = GetService<IDanhMucService>().GetOneCongTy(NhanVien.CongTyID).GetAwaiter().GetResult();
        //            }
        //        }
        //        return _congTy;
        //    }
        //}
    }
}
