using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Configuration;

public class SLDDataApiConfiguration : IAccessTokenApiConfiguration
{
    public string Url { get; set; } = string.Empty;
    public AccessTokenProviderApiConfiguration TokenSettings { get; set; } = new();
}
