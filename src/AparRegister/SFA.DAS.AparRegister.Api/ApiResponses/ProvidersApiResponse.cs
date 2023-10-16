using System.Collections.Generic;
using System.Linq;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;
using SFA.DAS.AparRegister.InnerApi.Responses;

namespace SFA.DAS.AparRegister.Api.ApiResponses
{
    /// <summary>
    /// ProviderList
    /// </summary>
    public class ProvidersApiResponse
    {
        /// <summary>
        /// Providers
        /// </summary>
        public IEnumerable<ProviderApiResponse> Providers { get; set; }
        public static implicit operator ProvidersApiResponse(GetProvidersQueryResult source)
        {
            return new ProvidersApiResponse
            {
                Providers = source.RegisteredProviders.Select(c=>(ProviderApiResponse)c).ToList()
            };
        }
    }

    /// <summary>
    /// Provider
    /// </summary>
    public class ProviderApiResponse
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
        public bool CanAccessApprenticeshipService { get; set; }
        public static implicit operator ProviderApiResponse(RegisteredProvider source)
        {
            return new ProviderApiResponse
            {
                Ukprn = source.Ukprn,
                Name = source.Name,
                TradingName = source.TradingName,
                CanAccessApprenticeshipService = source.CanAccessApprenticeshipService,
                StatusId = source.StatusId,
                ProviderTypeId = source.ProviderTypeId
            };
        }
    }
}