using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Configure
{
    public static class ConfigureExtensions
    {
        private const string SettingPrefix = "Settings";
        public static string GetSetting(this IConfiguration configuration, string settingName)
        {
            return GetSetting<string>(configuration, settingName);
        }
        public static T GetSetting<T>(this IConfiguration configuration, string settingName)
        {
            return configuration.GetValue<T>($"{SettingPrefix}:{settingName}");
        }
        //public static T Get<T>(this IConfiguration config, string key) where T : new()
        //{
        //    var instance = new T();
        //    config.GetSection(key).Bind(instance);
        //    return instance;
        //}
    }
}
