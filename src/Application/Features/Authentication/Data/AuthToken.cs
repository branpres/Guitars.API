using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Data
{
    public class AuthToken
    {
        public string UserId { get; set; }

        public IdentityUser User { get; set; } 

        public string JwtId { get; set; }

        public string Token { get; set; }

        public bool IsUsable { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}