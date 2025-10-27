namespace StoreManagement.API.Modules.Authentication.Services
{
    using Microsoft.IdentityModel.Tokens;
    using StoreManagement.API.Modules.Authentication.Constants;
    using StoreManagement.API.Shared.Entities;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class JwtService
    {
        private readonly IConfiguration _config;

        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration config, ILogger<JwtService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateAccessToken(UserInfoClaim info)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
           new("sub", info.Id.ToString()),
           new("name", info.UserName),
           new("email", info.Email),
           new("role", info.Role.ToUpper())

        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public JwtToken GenerateToken(UserInfoClaim user)
        {
            return new JwtToken
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = GenerateRefreshToken(),
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = "name",        
                    RoleClaimType = "role"        
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return null;
            }
        }

        public TokenInfo DecodeToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(token))
                {
                    _logger.LogWarning("Cannot read token");
                    return null;
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);

                return new TokenInfo
                {
                    UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,        // 🔥 sub claim
                    Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value,    // 🔥 name claim
                    Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,      // 🔥 email claim
                    Role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value,        // 🔥 role claim
                    Expiry = jwtToken.ValidTo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding token");
                return null;
            }
        }

        public TokenInfo GetTokenInfo(HttpContext httpContext)
        {
            var token = GetTokenFromHttpContext(httpContext);

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return DecodeToken(token);
        }

        public string GetTokenFromHttpContext(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }

            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }
    }

    public class JwtToken
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
    }

    public class UserInfoClaim
    {
        public required string Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string Role { get; set; } = Roles.STAFF.ToString();
    }

    public class TokenInfo
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public DateTime Expiry { get; set; }
    }
}
