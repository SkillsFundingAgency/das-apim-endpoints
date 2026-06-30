using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;

public record GetUkrlpProvidersQueryResult(IEnumerable<ProviderDetails> Providers);

public class ProviderDetails
{
    public int Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public Address LegalAddress { get; set; }
    public ProviderContactModel PrimaryContact { get; set; }

    public static implicit operator ProviderDetails(UkrlpProviderModel providerDetailsModel)
        => new()
        {
            Ukprn = providerDetailsModel.Ukprn,
            LegalName = providerDetailsModel.LegalName,
            TradingName = providerDetailsModel.TradingName,
            LegalAddress = providerDetailsModel.LegalAddress,
            PrimaryContact = providerDetailsModel.ContactDetails
        };
}

