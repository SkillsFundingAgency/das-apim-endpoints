using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.SharedOuterApi.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Authentication;

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
