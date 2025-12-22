using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoListWebAPI.Models;

namespace ToDoListWebAPI.Services
{
    public class AuthService(IConfiguration config) : IAuthService
    {
        private readonly IConfiguration _config = config;
        private static readonly Dictionary<string, AuthToken> _refreshTokens = new();
        private static readonly object _lock = new();

        public AuthResponse Login(string username, string password)
        {
            // Hardcoded user for test purposes
            if (username != "admin" || password != "password")
                throw new UnauthorizedAccessException();

            return GenerateAuthResponse(username);
        }

        public AuthResponse Refresh(string username, string refreshToken)
        {
            AuthToken? existingRefreshToken = null;
            lock (_lock)
            {
                if (!_refreshTokens.TryGetValue(refreshToken, out existingRefreshToken))
                    throw new SecurityTokenException("Invalid refresh token");

                if (existingRefreshToken.ExpiresAt < DateTime.Now)
                {
                    _refreshTokens.Remove(refreshToken);
                    throw new SecurityTokenException("Refresh token expired");
                }
            }

            return GenerateAuthResponse(username, existingRefreshToken);
        }

        private AuthResponse GenerateAuthResponse(string username, AuthToken? existingRefreshToken = null)
        {
            AuthResponse res =  new() { AccessToken = GenerateJwtToken(username) };

            string refreshToken = string.Empty;
            if (existingRefreshToken != null)
            {
                res.RefreshToken = existingRefreshToken;
            }
            else
            {
                res.RefreshToken = GenerateRefreshToken();
            }
            return res;
        }

        private AuthToken GenerateJwtToken(string username)
        {
            var jwtSection = _config.GetSection("ServiceConfiguration:Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expiresAt = DateTime.Now.AddMinutes(int.Parse(jwtSection["TokenLifetime"]!));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new AuthToken() { Token = new JwtSecurityTokenHandler().WriteToken(token) , ExpiresAt = expiresAt };
        }

        private AuthToken GenerateRefreshToken()
        {
            var token = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64)
            );

            DateTime expiresAt = DateTime.Now.AddDays(_config.GetValue<int>("ServiceConfiguration:Jwt:RefreshTokenLifetime"));

            lock (_lock)
            {
                _refreshTokens[token] = new AuthToken
                {
                    Token = token,
                    ExpiresAt = expiresAt
                };
            }

            return new AuthToken() { Token = token, ExpiresAt = expiresAt };
        }
    }
}
