using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces
{
    public interface IAccessTokenApiConfiguration : IApiConfiguration
    {
        AccessTokenProviderApiConfiguration TokenSettings { get; set; }
    }
}
