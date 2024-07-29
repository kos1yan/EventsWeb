using Events.Application.DataTransferObjects.Token;
using Events.Application.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Entities.ConfigurationModels;
using Events.Domain.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Events.Infrastructure.Auth
{
    public sealed class TokenService : ITokenService
    {
        private readonly IRepositoryManager _repository;
        private readonly IOptions<JwtConfiguration> _configuration;
        private readonly JwtConfiguration _jwtConfiguration;

        public TokenService(IRepositoryManager repository, IOptions<JwtConfiguration> configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _jwtConfiguration = _configuration.Value;
        }
        public async Task<TokenDto> CreateToken(User user, bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            if (populateExp) user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _repository.User.UpdateUserAsync(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };

            var roles = await _repository.User.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("userRole", role));
            }

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenDto> RefreshToken(string refreshToken)
        {
            var user = await _repository.User.GetByRefreshTokenAsync(refreshToken);
            if (user is null || user.RefreshTokenExpiryTime <= DateTime.Now) throw new RefreshTokenBadRequestException();
            return await CreateToken(user, populateExp: false);
        }
    }
}
