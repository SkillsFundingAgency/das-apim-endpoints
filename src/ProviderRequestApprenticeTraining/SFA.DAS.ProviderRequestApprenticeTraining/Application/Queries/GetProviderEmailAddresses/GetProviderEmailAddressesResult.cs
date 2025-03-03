using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses
{
    public class GetProviderEmailAddressesResult
    {
        public List<string> EmailAddresses { get; set; }
    }
}
