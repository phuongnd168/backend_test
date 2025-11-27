using System;
using System.Threading.Tasks;
using TLS.Core.Service;

namespace TLS.Core.Captcha
{
    public interface ICaptchaService : IService
    {
        CaptchaInstance CreateCaptcha();
        bool Validate(string instanceToken, string userInput);
        string GetDisplayText(int position, string lang = null);
    }
}
