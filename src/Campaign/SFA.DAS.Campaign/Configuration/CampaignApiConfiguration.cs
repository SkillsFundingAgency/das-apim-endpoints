using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Configuration;

public class CampaignApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}
