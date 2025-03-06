using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FernandoLibrary.Presentation.Common;
using Microsoft.IdentityModel.Tokens;
using vortexUserConfig.UsersConfig.Presentation.Common;

namespace Services.JwtConfig;

public class JwtToken
{
    private readonly IConfiguration _config;
    
    public JwtToken(IConfiguration config)
    {
        _config = config;
    }
    
    public string GenerateToken(OneUserRepository oneUserRepository)
    {
        var key = Encoding.UTF8.GetBytes(_config.GetSection("Authentication:Key").Value!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, oneUserRepository.Email),
            new Claim(ClaimTypes.Role, oneUserRepository.Role!.Name),
            new Claim("roleId", oneUserRepository.Role.Id.ToString()),
            new Claim("id", oneUserRepository.Id.ToString())
        };
        
        var token = new JwtSecurityToken(
            issuer: _config["Authentication:Issuer"],
            audience: _config["Authentication:Audience"],
            claims: claims,
            expires: System.DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
    
}