using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Service;

namespace TLS.Legacy.OAuth.Connector
{
    public class OAuthConnectorService : ServiceBase<OAuthConnectorService>, IOAuthConnectorService
    {
        private const string HeaderKeyPartnerCredential = "x-api-basic";
        private readonly OAuthConnectorSettings _settings;
        public OAuthConnectorSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public OAuthConnectorService(IServiceProvider serviceProvider, IOptions<OAuthConnectorSettings> settings) : base(serviceProvider)
        {
            _settings = settings.Value;
        }
        public async Task<OAuthCheckUserOutput> CheckUser(OAuthCheckUserInput input)
        {
            input.Id = input.Id ?? 0;
            return await CallApi<OAuthCheckUserInput, OAuthCheckUserOutput>("/api/User/PartnerCheckUser", input);
        }
        public async Task<OAuthCreateUserOutput> CreateUser(OAuthCreateUserInput input)
        {
            if (!input.EmailConfirmed.HasValue)
            {
                input.EmailConfirmed = !string.IsNullOrEmpty(input.Email);
            }
            if (!input.PhoneNumberConfirmed.HasValue)
            {
                input.PhoneNumberConfirmed = !string.IsNullOrEmpty(input.PhoneNumber);
            }
            return await CallApi<OAuthCreateUserInput, OAuthCreateUserOutput>("/api/User/PartnerCreateUser", input);
        }

        public async Task<OAuthUpdateUserOutput> UpdateUser(OAuthUpdateUserInput input)
        {
            if (!input.EmailConfirmed.HasValue)
            {
                input.EmailConfirmed = !string.IsNullOrEmpty(input.Email);
            }
            if (!input.PhoneNumberConfirmed.HasValue)
            {
                input.PhoneNumberConfirmed = !string.IsNullOrEmpty(input.PhoneNumber);
            }
            return await CallApi<OAuthUpdateUserInput, OAuthUpdateUserOutput>("/api/User/PartnerUpdateUser", input);
        }
        private string GetCredential(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"));
        }

        private async Task<TOut> CallApi<TIn,TOut>(string api, TIn input)
        {
            var apiUrl = string.Format("{0}/{1}", Settings.ApiUrl.TrimEnd('/'), api.TrimStart('/'));
            var credential = GetCredential(Settings.UserName, Settings.Password);
            var jsonInput = JsonConvert.SerializeObject(input);
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(3);
            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderKeyPartnerCredential, credential);
            var res = await client.PostAsync(apiUrl, new StringContent(jsonInput, Encoding.UTF8, "application/json"));
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TOut>(content);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                throw new Exception(string.Format("Error {0}({1}) when invoke api {2} with input {3}", res.StatusCode.GetHashCode(), content, apiUrl, jsonInput));
            }
        }
    }
}
