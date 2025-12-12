using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;

namespace TLS.BHL.Infra.Business.Services
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IAuthRepository AuthRepo;
        public AuthService(IServiceProvider serviceProvider, IAuthRepository authRepo) : base(serviceProvider)
        {
            AuthRepo = authRepo;
        }

        public async Task<ApiResponse> ChangePasswordForgot(string password, string confirmPassword, CancellationToken cancellationToken)
        {
            return await AuthRepo.ChangePasswordForgot(password, confirmPassword, cancellationToken);
        }

        public async Task<ApiResponse> ForgotPassword(string otp, string email, CancellationToken cancellationToken)
        {
            return await AuthRepo.ForgotPassword(otp, email, cancellationToken);
        }

        public async Task<ApiResponse> Login(LoginDTO login)
        {
            return await AuthRepo.Login(login);
        }

        public async Task<ApiResponse> Register(RegisterDTO register, CancellationToken cancellationToken)
        {
            return await AuthRepo.Register(register, cancellationToken);
        }

        public async Task<ApiResponse> SendOtp(string email, CancellationToken cancellationToken)
        {
            return await AuthRepo.SendOtp(email, cancellationToken);
        }
    }
}
