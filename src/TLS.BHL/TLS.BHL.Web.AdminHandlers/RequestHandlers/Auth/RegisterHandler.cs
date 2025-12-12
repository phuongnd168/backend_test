using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.DTO.Category;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Services;

namespace TLS.BHL.Web.AdminHandlers.RequestHandlers.Auth
{
    public class RegisterHandler : WebAdminHandlersBase<RegisterHandler>, IRequestHandler<RegisterInput, ApiResponse>
    {

        private IAuthService AuthService => GetService<IAuthService>();
        public RegisterHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ApiResponse> Handle(RegisterInput request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.register.Email))
                return ResponseHelper.Error(400, "Email không được để trống");
            if (string.IsNullOrWhiteSpace(request.register.Password))
                return ResponseHelper.Error(400, "Password không được để trống");
            if (string.IsNullOrWhiteSpace(request.register.FullName))
                return ResponseHelper.Error(400, "Full Name không được để trống");
         
            var patternMail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var mail = Regex.IsMatch(request.register.Email, patternMail, RegexOptions.IgnoreCase);
            if (!mail)
            {
                return ResponseHelper.Error(400, "Email không hợp lệ");
            }
            var patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            var pass = Regex.IsMatch(request.register.Password, patternPass, RegexOptions.IgnoreCase);
            if (!pass)
            {
                return ResponseHelper.Error(400, "Mật khẩu dài ít nhất 6 ký tự và phải có chữ hoa, chữ thường, số");
            }
            if (request.register.Password != request.register.ConfirmPassword)
            {
                return ResponseHelper.Error(400, "Password nhập lại không khớp");
            }
            return await AuthService.Register(request.register, cancellationToken);
        }
    }
    public class RegisterInput : IRequest<ApiResponse>
    {
        public RegisterDTO register { get; set; } 
    }
}
