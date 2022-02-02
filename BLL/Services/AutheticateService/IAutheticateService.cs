using System.Threading.Tasks;
using DTO;

namespace BLL.Services
{
    public interface IAutheticateService
    {
        Task<AuthenticateResponseDto> Login(AutheticateDto mode,string ipAddress);

        Task<ResponseDto> Register(RegisterDto model);

        Task VerifyEmail(string userId, string token);

        Task<AuthenticateResponseDto> RefreshToken(string token, string ip);

        Task<AuthenticateResponseDto> LoginWithFacebook(string accessToken, string ipAddress);
    }
}