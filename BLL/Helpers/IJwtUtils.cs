using System.Security.Claims;
using DAL.Entities;
using Microsoft.Extensions.Configuration;

namespace BLL.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateAccessToken(ApplicationUser identityUser, string role);

        public ClaimsIdentity GetIdentity(ApplicationUser person, string role);

        public RefreshToken GenerateRefreshToken(IConfiguration configuration,ApplicationUser user,string ipAddress);
    }
}