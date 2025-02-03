using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IAccessTokenApiConfiguration : IApiConfiguration
    {
        AccessTokenProviderApiConfiguration TokenSettings { get; set; }
    }
}
