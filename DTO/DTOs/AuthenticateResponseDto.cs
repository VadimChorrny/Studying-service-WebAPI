using System.Collections.Generic;
using DAL.Entities;

namespace DTO
{
    public class AuthenticateResponseDto
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}