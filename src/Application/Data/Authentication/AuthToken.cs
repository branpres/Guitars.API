using Microsoft.AspNetCore.Identity;

namespace Application.Data.Authentication
{
    public class AuthToken
    {
        public string UserId { get; set; }

        public IdentityUser User { get; set; } 

        public string JwtId { get; set; }

        public bool IsUsable { get; set; } = true;

        public bool IsRevoked { get; set; } = false;

        public DateTime RefreshTokenExpiresOn { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}