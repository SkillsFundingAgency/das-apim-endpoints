using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
public class GetProviderSummaryQueryResponse
{
    public int Ukprn { get; set; }
    public string Name { get; set; }
    public string TradingName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ContactUrl { get; set; }
    public int ProviderTypeId { get; set; }
    public int StatusId { get; set; }
    public string MarketingInfo { get; set; }
    public bool CanAccessApprenticeshipService { get; set; }
    public ProviderAddressModel Address { get; set; }
    public ProviderQarModel Qar { get; set; }
    public ReviewsModel Reviews { get; set; }
}
