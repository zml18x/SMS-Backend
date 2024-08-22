using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;

namespace SpaManagementSystem.Infrastructure.Services;

/// <summary>
/// Service responsible for creating JWT tokens for user authentication.
/// This class handles the generation of JWTs using user details and the application's JWT configuration settings.
/// </summary>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;


        
    /// <summary>
    /// Initializes a new instance of the JwtService with the application's configuration.
    /// </summary>
    /// <param name="configuration">Configuration properties, typically from app settings.</param>
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        ValidateConfiguration();
    }


        
    /// <inheritdoc />
    public JwtDto CreateToken(Guid userId, string userEmail, IList<string> userRoles)
    {
        try
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be null", nameof(userEmail));


            var token = CreateJwtToken(CreateClaims(userId, userEmail, userRoles), CreateSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwt = tokenHandler.WriteToken(token);

            return new JwtDto(jwt, token.ValidTo);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create JWT token.", ex);
        }
    }
    
    private List<Claim> CreateClaims(Guid userId, string userEmail, IList<string> userRoles)
    {
        var jwtSub = _configuration.GetSection("JWT:JwtRegisteredClaimNamesSub").Value!;


        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
            new Claim(ClaimTypes.Name,userId.ToString()),
            new Claim(ClaimTypes.Email,userEmail)
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
        var expire = DateTime.UtcNow.AddMinutes(int.Parse(_configuration.GetSection("JWT:ExpiryMinutes").Value!));
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

        if (!int.TryParse(_configuration["JWT:ExpiryMinutes"], out int expiryMinutes) || expiryMinutes < 0)
            throw new InvalidOperationException("JWT ExpiryMinutes is not a valid positive integer.");
    }
}