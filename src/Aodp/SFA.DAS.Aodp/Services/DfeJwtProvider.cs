using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.Aodp.Configuration;

namespace SFA.DAS.Aodp.Services;
public interface IDfeJwtProvider
{
    public string CreateToken();
}

public sealed class DfeJwtProvider : IDfeJwtProvider
{
    private readonly DfeSignInApiConfiguration _cfg;

    public DfeJwtProvider(IOptions<DfeSignInApiConfiguration> cfg) => _cfg = cfg.Value;

    public string CreateToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.ApiSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(_cfg.TokenLifetimeMinutes <= 0 ? 5 : _cfg.TokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer: _cfg.ClientId,
            audience: _cfg.Audience,
            notBefore: DateTime.UtcNow.AddSeconds(-10),
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}