using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BLL.Helpers;
using DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace BLL.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private const string fields = "first_name,last_name,email";
        private const string TokenValidUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private static readonly string UserInfoUrl = "https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={0}";
        private readonly FacebookAuthSettings facebookAuthSettings;
        private readonly IHttpClientFactory httpClientFactory;
        
        public FacebookAuthService(IHttpClientFactory httpClientFactory,IOptions<FacebookAuthSettings> facebookAuthSettings)
        {
            this.facebookAuthSettings = facebookAuthSettings.Value;
            this.httpClientFactory = httpClientFactory;
        }
        
        public async Task<bool> ValidateAccessToken(string accessToken)
        {
            var formattedUrl = string.Format(TokenValidUrl,
                                             accessToken,
                                             facebookAuthSettings.AppId,
                                             facebookAuthSettings.AppSecret);

            var result = await httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<FacebookTokenValidationResult.Root>(responseAsString);
            return res.data.is_valid;
        }

        public async Task<FacebookUserInfoDto> GetUserInfo(string accessToken)
        {
            var formattedUrl = string.Format(UserInfoUrl, accessToken);
            var result = await httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfoDto>(responseAsString);
        }
    }
}