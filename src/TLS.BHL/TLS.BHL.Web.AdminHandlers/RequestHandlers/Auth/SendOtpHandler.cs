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
    public class SendOtpHandler : WebAdminHandlersBase<SendOtpHandler>, IRequestHandler<SendOtpInput, ApiResponse>
    {

        private IAuthService AuthService => GetService<IAuthService>();
        public SendOtpHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(SendOtpInput request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Email))
                return ResponseHelper.Error(400, "Email không được để trống");
       
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var mail = Regex.IsMatch(request.Email, pattern, RegexOptions.IgnoreCase);
            if (!mail)
            {
                return ResponseHelper.Error(400, "Email không hợp lệ");
            }

            return await AuthService.SendOtp(request.Email, cancellationToken);
        }
    }
    public class SendOtpInput : IRequest<ApiResponse>
    {
        public string Email { get; set; } 
    }
}
