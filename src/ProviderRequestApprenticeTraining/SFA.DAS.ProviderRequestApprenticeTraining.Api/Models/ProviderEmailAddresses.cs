using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class ProviderEmailAddresses
    {
        public List<string> EmailAddresses { get; set; }

        public static implicit operator ProviderEmailAddresses(GetProviderEmailAddressesResult source)
        {
            return new ProviderEmailAddresses
            {
                EmailAddresses = source.EmailAddresses,
            };
        }
    }
}
