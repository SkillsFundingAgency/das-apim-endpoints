using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

public class LearnerDataApiConfiguration : IAccessTokenApiConfiguration
{
    public string Url { get; set; }
    public AccessTokenProviderApiConfiguration TokenSettings { get; set; }
}