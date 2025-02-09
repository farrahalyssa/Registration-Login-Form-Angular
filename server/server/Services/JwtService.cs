using server.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace server.Services
{
    public class JwtService
    {
        private readonly JwtConfig? _jwtConfig;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        // Constructor to inject the configuration and logger
        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>(); 
        }

        // Generate a JWT token
        public string GenerateJwtToken(User user)
        {
            if (_jwtConfig == null)
                throw new InvalidOperationException("JWT configuration is not properly set.");

            if (string.IsNullOrEmpty(_jwtConfig.Key))
            {
                throw new InvalidOperationException("JWT key is not provided in the configuration.");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);

            // Claims to be added to the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.userId.ToString())
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer ?? throw new InvalidOperationException("Issuer is not provided in the configuration."),
                audience: _jwtConfig.Audience ?? throw new InvalidOperationException("Audience is not provided in the configuration."),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate the JWT token
       public ClaimsPrincipal? ValidateToken(string token)
{
    if (string.IsNullOrEmpty(_jwtConfig?.Key))
    {
        throw new InvalidOperationException("JWT key is not provided in the configuration.");
    }

    var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
    var tokenHandler = new JwtSecurityTokenHandler();

    var validationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _jwtConfig.Issuer,
        ValidAudience = _jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    try
    {
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        if (validatedToken is JwtSecurityToken jwtToken)
        {
            if (jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return principal; // Return the ClaimsPrincipal if the token is valid
            }
        }

        return null; // Invalid token, return null
    }
    catch (SecurityTokenExpiredException)
    {
        return null; // Return null if the token has expired
    }
    catch (Exception)
    {
        return null; // Return null for any other validation errors
    }
}

    }

}
