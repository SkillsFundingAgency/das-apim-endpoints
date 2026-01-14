using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.SharedOuterApi.Models.Roatp;
public class ProviderDetailsModel
{
    public Guid Id { get; set; }
    public long Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public ProviderType ProviderType { get; set; }

    public bool IsMainProvider => ProviderType == ProviderType.Main;

    public static implicit operator ProviderDetailsModel(OrganisationResponse source)
    {
        if (source == null)
        {
            return null;
        }
        return new ProviderDetailsModel
        {
            Id = source.OrganisationId,
            Ukprn = source.Ukprn,
            LegalName = source.LegalName,
            TradingName = source.TradingName,
            ProviderType = source.ProviderType
        };
    }
}
