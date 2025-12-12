using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;


namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class AuthRepository : BHLRepositoryBase<AuthRepository>, IAuthRepository
    {
        private readonly IBHLDbContext Context;
        private readonly IJwtService _jwt;
        private readonly IEmailService _email;
        public AuthRepository(IServiceProvider serviceProvider, IBHLDbContext context, IJwtService jwt, IEmailService emailService) : base(serviceProvider)
        {
            Context = context;
            _jwt = jwt;
            _email = emailService;
        }

        public async Task<ApiResponse> ChangePasswordForgot(string password, string resetToken, CancellationToken cancellationToken)
        {
            var check = await Context.ForgotPassword.Where(x => x.resetToken == resetToken).FirstOrDefaultAsync();
            if (check == null) 
            {
                return ResponseHelper.Error(404, "Reset token không tồn tại");
            }
            if (check.ExpiredResetTokenAt < DateTime.Now)
            {
                return ResponseHelper.Error(400, "Reset token đã hết hạn");
            }
            var user = await Context.Users.Where(x => x.Id == check.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                return ResponseHelper.Error(404, "Tài khoản không tồn tại");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            var result = await Context.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                Context.ForgotPassword.Remove(check);
                await Context.SaveChangesAsync(cancellationToken);
                return ResponseHelper.Success("Đổi mật khẩu thành công");
            }

            return ResponseHelper.Error(500, "Lỗi");
        }

        public async Task<ApiResponse> ForgotPassword(string otp, string email, CancellationToken cancellationToken)
        {
            var mail = await Context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (mail == null) {
                return ResponseHelper.Error(404, "Email không tồn tại");
            }
            var check = await Context.ForgotPassword.Where(x => x.Otp == otp && x.UserId == mail.Id).FirstOrDefaultAsync();
            if (check == null)
            {
                return ResponseHelper.Error(404, "Mã otp không tồn tại");
            }
            if (check.ExpiredOtpAt < DateTime.Now)
            {
                return ResponseHelper.Error(400, "Mã otp đã hết hạn");
            }
            check.Otp = "000000";
            check.resetToken = Guid.NewGuid().ToString();
            check.ExpiredResetTokenAt = DateTime.Now.AddMinutes(5);
            var result = await Context.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResponseHelper.Success("Thành công", new {resetToken = check.resetToken});
            }

            return ResponseHelper.Error(500, "Lỗi");
        }

        public async Task<ApiResponse> Login(LoginDTO login)
        {

            var email = login.Email.Trim().ToLowerInvariant();
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email);
            if (user == null)
            {
                return ResponseHelper.Error(400, "Sai tài khoản hoặc mật khẩu");
            }
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return ResponseHelper.Error(400, "Sai tài khoản hoặc mật khẩu");
            }
            var token = _jwt.GenerateToken(user);
            if (token!=null)
            {
                return ResponseHelper.Success("Đăng nhập thành công",  new { Token = token, User = new { user.Id, user.Email, user.FullName } });
           
            }
            return ResponseHelper.Error(400, "Đăng nhập thất bại");

        }

        public async Task<ApiResponse> Register(RegisterDTO register, CancellationToken cancellationToken)
        {
            
            var email = register.Email.Trim().ToLowerInvariant();
            var checkEmail = await Context.Users.AnyAsync(u => u.Email == email);
            if (checkEmail)
            {
                return ResponseHelper.Error(400, "Email đã tồn tại");
            }
                

            var user = new UserEntity
            {
                UserId = Guid.NewGuid().ToString(),
                Email = email,
                FullName = register.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Created_at = DateTime.Now
            };
            await Context.Users.AddAsync(user);
            var result = await Context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return ResponseHelper.Created("Tạo tài khoản thành công");
            }
            return ResponseHelper.Error(500, "Tạo tài khoản thất bại");
        }

        public async Task<ApiResponse> SendOtp(string email, CancellationToken cancellationToken)
        {

            var mail = await Context.Users.Where(x=>x.Email==email).FirstOrDefaultAsync();
            if (mail == null)
            {
                return ResponseHelper.Error(404, "Email không tồn tại");
            }
            var otp = new Random().Next(100000, 999999).ToString();
            string subject = "Mã OTP của bạn";
            string body = $"<h2>OTP: {otp}</h2>";

            var result = await _email.SendEmailAsync(email, subject, body);
            if (result)
            {
                var checkOtp = await Context.ForgotPassword.Where(x => x.UserId == mail.Id).FirstOrDefaultAsync();
                if (checkOtp !=null)
                {
                    checkOtp.Otp = otp;
                    checkOtp.ExpiredOtpAt = DateTime.Now.AddMinutes(5);
                    checkOtp.Updated_at = DateTime.Now;

                    var update = await Context.SaveChangesAsync(cancellationToken);
                    if (update > 0)
                    {
                        return ResponseHelper.Updated("Thành công");
                    }
                    return ResponseHelper.Error(400, "Thất bại");
                }
                var createOtp = new ForgotPasswordEntity
                {
                    Otp = otp,
                    ExpiredOtpAt = DateTime.Now.AddMinutes(5),
                    Created_at = DateTime.Now,
                    UserId = mail.Id
                };
                await Context.ForgotPassword.AddAsync(createOtp);
                var check = await Context.SaveChangesAsync(cancellationToken);
                if (check > 0)
                {
                    return ResponseHelper.Created("Thành công");
                }
                return ResponseHelper.Error(400, "Thất bại");
            }
            return ResponseHelper.Error(500, "Lỗi");
        }
    }
}
