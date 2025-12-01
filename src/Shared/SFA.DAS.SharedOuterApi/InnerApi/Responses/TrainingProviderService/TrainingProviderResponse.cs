using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;

[Obsolete("Use SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.ProviderDetailsResponse instead")]
public class TrainingProviderResponse
{
    public TrainingProviderResponse()
    {
        ProviderType = new ProviderTypeResponse();
    }

    public Guid Id { get; set; }
    public long Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public ProviderTypeResponse ProviderType { get; set; }

    public bool IsMainProvider => ProviderType.Id == (short)ProviderTypeIdentifier.MainProvider;

    public static implicit operator TrainingProviderResponse(OrganisationResponse source)
    {
        if (source == null)
        {
            return null;
        }
        return new TrainingProviderResponse
        {
            Id = source.OrganisationId,
            Ukprn = source.Ukprn,
            LegalName = source.LegalName,
            TradingName = source.TradingName,
            ProviderType = new ProviderTypeResponse { Id = (short)source.ProviderType }
        };
    }

    public class ProviderTypeResponse
    {
        public short Id { get; set; }
    }

    [Obsolete("Use SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common.ProviderType instead")]
    public enum ProviderTypeIdentifier : short
    {
        MainProvider = 1,
        EmployerProvider = 2,
        SupportingProvider = 3
    }
}
