using MediatR;
using System;
using System.Collections.Generic;
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
    public class ForgotPasswordHandler : WebAdminHandlersBase<ForgotPasswordHandler>, IRequestHandler<ForgotPasswordInput, ApiResponse>
    {

        private IAuthService AuthService => GetService<IAuthService>();
        public ForgotPasswordHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(ForgotPasswordInput request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Otp))
                return ResponseHelper.Error(400, "Mã OTP không được để trống");
            if (string.IsNullOrWhiteSpace(request.Email))
                return ResponseHelper.Error(400, "Email không được để trống");
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var mail = Regex.IsMatch(request.Email, pattern, RegexOptions.IgnoreCase);
            if (!mail)
            {
                return ResponseHelper.Error(400, "Email không hợp lệ");
            }


            return await AuthService.ForgotPassword(request.Otp, request.Email, cancellationToken);
        }
    }
    public class ForgotPasswordInput : IRequest<ApiResponse>
    {
        public string Otp { get; set; }
        public string Email { get; set; }
    }
}
