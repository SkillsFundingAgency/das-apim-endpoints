using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RequestApprenticeTraining;

[ExcludeFromCodeCoverage]
public class GetSettingsResponse
{
    public int ExpiryAfterMonths { get; set; }
    public int EmployerRemovedAfterExpiryMonths { get; set; }
    public int ProviderRemovedAfterRequestedMonths { get; set; }
}