using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Repositories
{
    public interface IAuthRepository : IRepository
    {
        Task<ApiResponse> ForgotPassword(string otp, string email, CancellationToken cancellationToken);
        Task<ApiResponse> Register(RegisterDTO register, CancellationToken cancellationToken);
        Task<ApiResponse> Login(LoginDTO login);

        Task<ApiResponse> SendOtp(string email, CancellationToken cancellationToken);
        Task<ApiResponse> ChangePasswordForgot(string password, string resetToken, CancellationToken cancellationToken);
    }
}
