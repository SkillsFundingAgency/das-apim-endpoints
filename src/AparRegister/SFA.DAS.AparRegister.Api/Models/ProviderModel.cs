using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AparRegister.Api.Models;

/// <summary>
/// Provider
/// </summary>
public class ProviderModel
{
    /// <summary>
    /// UKPRN of provider
    /// </summary>
    public int Ukprn { get; set; }
    /// <summary>
    /// Registered name of provider
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Registered Trading name of provider
    /// </summary>
    public string TradingName { get; set; }
    /// <summary>
    /// Provider Type
    /// `0 -> Main`
    /// `1 -> Employer`
    /// `2 -> Supporting`
    /// </summary>
    public long ProviderTypeId { get; set; }
    /// <summary>
    /// Provider Status
    /// `0 -> Active`
    /// `1 -> ActiveButNotTakingOnApprentices`
    /// `2 -> OnBoarding`
    /// </summary>
    public long StatusId { get; set; }
    /// <summary>
    /// Set to true if Status is Active or Onboarding *AND* ProviderTypeId is Main or Employer
    /// </summary>
    public bool CanAccessApprenticeshipService => (ProviderType)ProviderTypeId is ProviderType.Main or ProviderType.Employer;

    /// <summary>
    /// Converts an instance of <see cref="OrganisationResponse"/> to a <see cref="ProviderModel"/> using an implicit
    /// cast.
    /// </summary>
    /// <remarks>The conversion maps key properties from <paramref name="source"/> to the corresponding fields
    /// in <see cref="ProviderModel"/>. This operator enables seamless assignment of an <see
    /// cref="OrganisationResponse"/> to a <see cref="ProviderModel"/> without explicit casting.</remarks>
    /// <param name="source">The <see cref="OrganisationResponse"/> instance to convert to a <see cref="ProviderModel"/>.</param>
    public static implicit operator ProviderModel(OrganisationResponse source)
    {
        return new ProviderModel
        {
            Ukprn = source.Ukprn,
            Name = source.LegalName,
            TradingName = source.TradingName,
            StatusId = (int)source.Status,
            ProviderTypeId = (int)source.ProviderType
        };
    }
}
