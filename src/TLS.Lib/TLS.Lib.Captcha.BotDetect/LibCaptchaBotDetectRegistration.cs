using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core;
using TLS.Core.Captcha;

namespace TLS.Lib.Captcha.BotDetect
{
    public static class LibCaptchaBotDetectRegistration
    {
        private const string DefaultCaptchaConfigKey = "CaptchaBotDetect";

        /// <summary>
        /// Add lib captcha use BotDetect provider use default config section ["CaptchaBotDetect"]
        /// </summary>
        /// <param name="services">required</param>
        /// <returns></returns>
        public static IServiceCollection AddLibCaptchaBotDetect(this IServiceCollection services)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Use default section
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(DefaultCaptchaConfigKey);
            if (!section.Exists())
            {
                throw new Exception($"Configuration section [{DefaultCaptchaConfigKey}] was not found");
            }

            return AddLibCaptchaBotDetect(services, section);
        }

        /// <summary>
        /// Add lib captcha use BotDetect provider use setup action of captcha BotDetect settings
        /// </summary>
        /// <param name="services">required</param>
        /// <param name="setupCaptchaSettingsOptions">required</param>
        /// <returns></returns>
        public static IServiceCollection AddLibCaptchaBotDetect(this IServiceCollection services, Action<CaptchaSettings> setupCaptchaSettingsOptions)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (setupCaptchaSettingsOptions == null)
            {
                throw new ArgumentNullException(nameof(setupCaptchaSettingsOptions));
            }

            //var captchaSettings = new CaptchaSettings();
            //if (options != null)
            //{
            //    options.Invoke(captchaSettings);
            //}

            services.Configure(setupCaptchaSettingsOptions);
            services.AddTransient<ICaptchaService, CaptchaBotDetectService>();

            return services;
        }

        /// <summary>
        /// Add lib captcha use BotDetect provider use special configuration section
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLibCaptchaBotDetect(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            // Check param
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Check param
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            services.Configure<CaptchaSettings>(configurationSection);
            services.AddTransient<ICaptchaService, CaptchaBotDetectService>();

            return services;
        }
    }
}
