using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;

namespace SpaManagementSystem.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;


    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        ValidateConfiguration();
    }


    
    public JwtDto CreateJwtToken(UserDto user)
    {
        try
        {
            if (user.Id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(user.Id));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("User email cannot be null", nameof(user.Email));


            var token = CreateJwtToken(CreateClaims(user.Id, user.Email, user.Roles), CreateSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwt = tokenHandler.WriteToken(token);

            return new JwtDto(jwt, token.ValidTo);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create JWT token.", ex);
        }
    }

    public RefreshTokenDto CreateRefreshToken(Guid userId)
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);

        var expirationTime =
            DateTime.UtcNow.AddDays(int.Parse(_configuration.GetSection("JWT:RefreshTokenExpirationDays").Value!));

        return new RefreshTokenDto(userId, Convert.ToBase64String(randomBytes), expirationTime);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid access token.");

        return principal;
    }

    private List<Claim> CreateClaims(Guid userId, string userEmail, IList<string> userRoles)
    {
        var jwtSub = _configuration.GetSection("JWT:JwtRegisteredClaimNamesSub").Value!;


        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, jwtSub),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new (ClaimTypes.NameIdentifier,userId.ToString()),
            new (ClaimTypes.Name,userId.ToString()),
            new (ClaimTypes.Email,userEmail)
        };

        if (userRoles.Any())
            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }
    
    private SigningCredentials CreateSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    }
    
    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials signingCredentials)
    {
        var expire = DateTime.UtcNow.AddMinutes(int.Parse(_configuration.GetSection("JWT:JwtExpirationTime").Value!));
        var issuer = _configuration.GetSection("JWT:Issuer").Value;
        var audience = _configuration.GetSection("JWT:Audience").Value;

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: expire,
            issuer: issuer,
            audience: audience);

        return token;
    }
    
    private void ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(_configuration["JWT:Key"]))
            throw new InvalidOperationException("JWT Key is missing in configuration.");

        if (string.IsNullOrEmpty(_configuration["JWT:JwtRegisteredClaimNamesSub"]))
            throw new InvalidOperationException("JwtRegisteredClaimNamesSub is missing in configuration.");

        if (string.IsNullOrEmpty(_configuration["JWT:Issuer"]))
            throw new InvalidOperationException("JWT Issuer is missing in configuration.");

        if (string.IsNullOrEmpty(_configuration["JWT:Audience"]))
            throw new InvalidOperationException("JWT Audience is missing in configuration.");

        if (!int.TryParse(_configuration["JWT:JwtExpirationTime"], out int expiryMinutes) || expiryMinutes < 0)
            throw new InvalidOperationException("JWT ExpiryMinutes is not a valid positive integer.");
    }
}