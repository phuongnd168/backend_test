using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Application;

namespace TLS.Core
{
    public static class CoreRegistration
    {
        private const string DefaultApplicationSettingsSectionName = "Settings";
        private static ApplicationSettings _appSettings;
        internal static ApplicationSettings AppSettings 
        { 
            get
            {
                return _appSettings;
            }
        }
        private static IConfiguration _configuration;
        internal static IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }
        private static IHostEnvironment _environment;
        internal static IHostEnvironment Environment
        {
            get
            {
                return _environment;
            }
        }

        private static void ApplyCoreSettings(IHostApplicationBuilder builder, ApplicationSettings appSettings)
        {
            _configuration = builder.Configuration;
            _environment = builder.Environment;
            _appSettings = appSettings;
            if (_appSettings.Environment == null)
            {
                _appSettings.Environment = builder.Environment;
            }
        }


        public static IHostApplicationBuilder ConfigureAppCore(this IHostApplicationBuilder builder)
        {
            return ConfigureAppCore(builder, DefaultApplicationSettingsSectionName);
        }
        public static IHostApplicationBuilder ConfigureAppCore(this IHostApplicationBuilder builder, string settingsSectionName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(settingsSectionName))
            {
                throw new ArgumentNullException(nameof(settingsSectionName));
            }


            // Check param
            var settingsSection = builder.Configuration.GetSection(settingsSectionName);
            if (!settingsSection.Exists())
            {
                throw new Exception($"Configuration section [{settingsSection.Key}] for application settings was not found");
            }

            // Get application setting from configuration
            var appSettings = settingsSection.Get<ApplicationSettings>();

            // Apply core settings
            ApplyCoreSettings(builder, appSettings);

            // Configure ApplicationSettings use in application executing later
            builder.Services.Configure<ApplicationSettings>(settingsSection);
            builder.Services.PostConfigure<ApplicationSettings>(m => {
                m.Environment = builder.Environment;
            });

            return builder;
        }

        public static IHostApplicationBuilder ConfigureAppCore(this IHostApplicationBuilder builder, Action<ApplicationSettings> setupSettings)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupSettings == null)
            {
                throw new ArgumentNullException(nameof(setupSettings));
            }

            // Allication settings
            var appSettings = new ApplicationSettings();
            setupSettings(appSettings);
            ApplyCoreSettings(builder, appSettings);

            // Configure ApplicationSettings use in application executing later
            builder.Services.Configure(setupSettings);
            builder.Services.PostConfigure<ApplicationSettings>(m => {
                m.Environment = builder.Environment;
            });

            return builder;
        }

        public static IHostApplicationBuilder ConfigureAppCore(this IHostApplicationBuilder builder, IConfigurationSection settingsSection)
        {
            // Check param
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Check param
            if (settingsSection == null)
            {
                throw new ArgumentNullException(nameof(settingsSection));
            }

            // Check param
            if (!settingsSection.Exists())
            {
                throw new Exception($"Configuration section [{settingsSection.Key}] for application settings was not found");
            }

            // Get application setting from configuration
            var appSettings = settingsSection.Get<ApplicationSettings>();

            // Apply core settings
            ApplyCoreSettings(builder, appSettings);

            // Configure ApplicationSettings use in application executing later
            builder.Services.Configure<ApplicationSettings>(settingsSection);
            builder.Services.PostConfigure<ApplicationSettings>(m => {
                m.Environment = builder.Environment;
            });

            return builder;
        }

        /// <summary>
        /// Get Configuration, Notice that must invoke IHostApplicationBuilder.ConfigureAppCore to set configuration first
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            if (Configuration == null)
            {
                throw new Exception("Core configuration was not config, please invoke IHostApplicationBuilder.ConfigureAppCore in Program.cs to set configuration first.");
            }

            return Configuration;
        }

        /// <summary>
        /// Get Hosting environment, Notice that must invoke IHostApplicationBuilder.ConfigureAppCore to set configuration first
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IHostEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            if (Environment == null)
            {
                throw new Exception("Core configuration was not config, please invoke IHostApplicationBuilder.ConfigureAppCore in Program.cs to set hosting environment first.");
            }

            return Environment;
        }

        /// <summary>
        /// Get Application settings
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationSettings GetApplicationSettings(this IServiceCollection services)
        {
            if (AppSettings == null)
            {
                throw new Exception("Core configuration was not config, please invoke IHostApplicationBuilder.ConfigureAppCore in Program.cs to set application settings first.");
            }
            return AppSettings;
        }
    }
}
