using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class ProviderPhoneNumbers
    {
        public List<string> PhoneNumbers { get; set; }

        public static implicit operator ProviderPhoneNumbers(GetProviderPhoneNumbersResult source)
        {
            return new ProviderPhoneNumbers
            {
                PhoneNumbers = source.PhoneNumbers,
            };
        }
    }
}
