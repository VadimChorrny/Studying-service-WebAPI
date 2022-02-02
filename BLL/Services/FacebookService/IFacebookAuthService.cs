using System.Threading.Tasks;
using DTO;

namespace BLL.Services
{
    public interface IFacebookAuthService
    {
        Task<bool> ValidateAccessToken(string accessToken);

        Task<FacebookUserInfoDto> GetUserInfo(string accessToken);
    }
}