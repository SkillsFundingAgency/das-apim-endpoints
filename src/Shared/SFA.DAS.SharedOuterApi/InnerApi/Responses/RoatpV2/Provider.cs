namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

public class Provider
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
    /// Registered Email address of provider
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Registered Trading phone number of provider
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Registered Contact Url of provider
    /// </summary>
    public string ContactUrl { get; set; }

    /// <summary>
    /// Provider Type
    /// `1 -> Main`
    /// `2 -> Employer`
    /// `3 -> Supporting`
    /// </summary>
    public int ProviderTypeId { get; set; }

    /// <summary>
    /// Provider Status
    /// `1 -> Active`
    /// `2 -> ActiveButNotTakingOnApprentices`
    /// `3 -> OnBoarding`
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Set to true if Status is Active or On-boarding *AND* ProviderTypeId is Main or Employer
    /// </summary>
    public bool CanAccessApprenticeshipService { get; set; }

    /// <summary>
    /// Registered Address of provider
    /// </summary>
    public ProviderAddress Address { get; set; }
}
