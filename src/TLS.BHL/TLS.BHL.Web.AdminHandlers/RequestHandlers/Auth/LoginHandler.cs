using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Auth
{
    public class LoginHandler : WebAdminHandlersBase<LoginHandler>, IRequestHandler<LoginInput, ApiResponse>
    {

        private IAuthService AuthService => GetService<IAuthService>();
        public LoginHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(LoginInput request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.login.Email))
                return ResponseHelper.Error(400, "Email không được để trống");
            if (string.IsNullOrWhiteSpace(request.login.Password))
                return ResponseHelper.Error(400, "Password không được để trống");

            var patternMail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var mail = Regex.IsMatch(request.login.Email, patternMail, RegexOptions.IgnoreCase);
            if (!mail)
            {
                return ResponseHelper.Error(400, "Email không hợp lệ");
            }
            var patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            var pass = Regex.IsMatch(request.login.Password, patternPass, RegexOptions.IgnoreCase);
            if (!pass)
            {
                return ResponseHelper.Error(400, "Mật khẩu dài ít nhất 6 ký tự và phải có chữ hoa, chữ thường, số");
            }
       

            return await AuthService.Login(request.login);
        }
    }
    public class LoginInput : IRequest<ApiResponse>
    {
        public LoginDTO login { get; set; }
    }
}
