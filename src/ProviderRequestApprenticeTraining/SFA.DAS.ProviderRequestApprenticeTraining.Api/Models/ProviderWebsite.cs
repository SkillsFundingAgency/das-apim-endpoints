using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class ProviderWebsite
    {
        public string Website { get; set; }

        public static implicit operator ProviderWebsite(GetProviderWebsiteResult source)
        {
            return new ProviderWebsite
            {
                Website = source.Website,
            };
        }
    }
}
