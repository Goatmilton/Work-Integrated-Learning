using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }

    // Issues short-lived, signed JWTs for the API surface (mobile app,
    // external integrations). The MVC site keeps using cookie auth via
    // Identity's SignInManager; this exists alongside it as a second
    // authentication scheme rather than a replacement.
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured. Set it via user-secrets or an environment variable.");
            var issuer = jwtSection["Issuer"] ?? "WoodlandsApi";
            var audience = jwtSection["Audience"] ?? "WoodlandsClients";
            var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var m) ? m : 60;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Email ?? ""),
                new("FullName", user.FullName)
            };

            if (!string.IsNullOrWhiteSpace(user.Branch))
            {
                claims.Add(new Claim("Branch", user.Branch));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
