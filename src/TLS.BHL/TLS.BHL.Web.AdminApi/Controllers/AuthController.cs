using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TLS.BHL.Infra.App.Domain.DTO.Auth;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.BHL.Web.AdminHandlers.RequestHandlers.Auth;


namespace TLS.BHL.Web.AdminApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : WebAdminControllersBase<AuthController>
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ApiResponse> Register([FromBody] RegisterDTO input)
        {
            try
            {
                return await Mediator.Send(new RegisterInput { register = input}, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ApiResponse> Login([FromBody] LoginDTO input)
        {
            try
            {
                return await Mediator.Send(new LoginInput { login = input }, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ApiResponse> ForgotPassword([FromBody] ForgotPasswordInput input)
        {
            try
            {
                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<ApiResponse> SendOTP([FromBody] SendOtpInput input)
        {
            try
            {
                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost("change-password-forgot")]
        public async Task<ApiResponse> ChangePasswordForgot([FromBody] ChangePasswordForgotInput input)
        {
            try
            {
                return await Mediator.Send(input, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                throw LogError(ex);
            }
        }

    }

}
