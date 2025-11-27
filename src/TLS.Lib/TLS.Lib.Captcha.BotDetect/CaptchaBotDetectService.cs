using BotDetect;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Captcha;
using TLS.Core.Security.Cryptography;

namespace TLS.Lib.Captcha.BotDetect
{
    public class CaptchaBotDetectService : ICaptchaService
    {
        private const string DefaultLang = "vi";
        private readonly Random _rnd = new Random();
        private readonly IDictionary<string, IEnumerable<string>> _dicText = new Dictionary<string, IEnumerable<string>>(
            new KeyValuePair<string, IEnumerable<string>>[]{
                new KeyValuePair<string, IEnumerable<string>>("vi", new string[] {
                "Nhập ký tự ở vị trí thứ {0}",
                "Nhập ký tự ở vị trí số {0}"
                })
            }
        );
        private readonly CaptchaSettings Settings;
        private readonly string Lang;
        private static bool _isRegisteredHandler;
        private static string _captchaCode;

        public CaptchaBotDetectService(IOptions<CaptchaSettings> options)
        {
            Settings = options.Value;
            ParseDicText(Settings.DicText);
            Lang = string.IsNullOrEmpty(Settings.Lang) ? DefaultLang : Settings.Lang;
            RegisterGeneratedCaptchaCodeHandler();
        }
        public CaptchaInstance CreateCaptcha()
        {
            //var captchaId = typeof(CaptchaBotDetectService).FullName;
            var captchaId = "DsvnCaptcha";
            var captcha = new CaptchaBase(captchaId);
            captcha.CodeLength = Settings.CodeLength;
            var instanceId = captcha.InstanceId;

            var image = captcha.GetImage(instanceId);
            if (image == null)
            {
                throw new Exception("Can not create captcha image.");
            }

            var position = _rnd.Next(1, Settings.CodeLength);
            var expiredDate = DateTime.Now.AddMinutes(Settings.ExpiresIn);
            var instance = new CaptchaInstance {
                InstanceId = instanceId,
                ExpiredDate = expiredDate,
                FullCode = _captchaCode,
                Position = position,
                Image = image
            };
            var rsa = new RSAHelper(RSAType.RSA2, Encoding.UTF8, null, Settings.PublicKey);
            instance.InstanceToken = rsa.Encrypt(JsonConvert.SerializeObject(instance));

            return instance;
        }

        public bool Validate(string instanceToken, string userInput)
        {
            var rsa = new RSAHelper(RSAType.RSA2, Encoding.UTF8, Settings.PrivateKey, null);
            var instanceJson = rsa.Decrypt(instanceToken);
            var instance = JsonConvert.DeserializeObject<CaptchaInstance>(instanceJson);

            // instance not found or user input empty
            if (instance == null || string.IsNullOrEmpty(instance.RealCode) || string.IsNullOrEmpty(userInput))
            {
                return false;
            }

            // If not correct with FullCode or RealCode
            if (!string.Equals(userInput, instance.FullCode, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(userInput, instance.RealCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // If expired
            if (DateTime.Now > instance.ExpiredDate)
            {
                return false;
            }

            // Final success
            return true;
        }

        public string GetDisplayText(int position, string lang = null)
        {
            lang = string.IsNullOrEmpty(lang) ? Lang : lang;
            var arrText = _dicText[lang];
            return string.Format(arrText.ElementAt(_rnd.Next(0, arrText.Count() - 1)), position);
        }

        private void ParseDicText(IDictionary<string, IEnumerable<string>> arrText)
        {
            if (Settings.DicText != null && Settings.DicText.Count > 0)
            {
                foreach (var item in arrText)
                {
                    if (_dicText.ContainsKey(item.Key))
                    {
                        _dicText[item.Key] = item.Value;
                    }
                    else
                    {
                        _dicText.Add(item);
                    }
                }
            }
        }

        private static void GeneratedCaptchaEvent(object sender, GeneratedCaptchaCodeEventArgs e)
        {
            _captchaCode = e.Code;
        }

        private static void RegisterGeneratedCaptchaCodeHandler()
        {
            if (!_isRegisteredHandler)
            {
                CaptchaBase.RegisterGeneratedCaptchaCodeHandler(GeneratedCaptchaEvent);
                _isRegisteredHandler = true;
            }
        }
    }
}
