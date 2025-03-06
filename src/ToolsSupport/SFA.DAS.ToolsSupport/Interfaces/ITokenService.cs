using SFA.DAS.ToolsSupport.Application.Services;

namespace SFA.DAS.ToolsSupport.Interfaces;

public interface ITokenService
{
    Task<PrivilegedAccessToken> GetPrivilegedAccessTokenAsync();
}
