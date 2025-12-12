using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Auth
{
    public class ChangePasswordForgotHandler : WebAdminHandlersBase<ChangePasswordForgotHandler>, IRequestHandler<ChangePasswordForgotInput, ApiResponse>
    {

        private IAuthService AuthService => GetService<IAuthService>();
        public ChangePasswordForgotHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(ChangePasswordForgotInput request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Password))
                return ResponseHelper.Error(400, "Password không được để trống");
            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                return ResponseHelper.Error(400, "Confirm Password không được để trống");
            if (string.IsNullOrWhiteSpace(request.resetToken))
                return ResponseHelper.Error(400, "Reset Token không được để trống");

            var patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            var pass = Regex.IsMatch(request.Password, patternPass, RegexOptions.IgnoreCase);
            if (!pass)
            {
                return ResponseHelper.Error(400, "Mật khẩu dài ít nhất 6 ký tự và phải có chữ hoa, chữ thường, số");
            }
            if (request.Password != request.ConfirmPassword)
            {
                return ResponseHelper.Error(400, "Password nhập lại không khớp");
            }

            return await AuthService.ChangePasswordForgot(request.Password, request.resetToken, cancellationToken);
        }
    }
    public class ChangePasswordForgotInput : IRequest<ApiResponse>
    {
        public string resetToken { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
