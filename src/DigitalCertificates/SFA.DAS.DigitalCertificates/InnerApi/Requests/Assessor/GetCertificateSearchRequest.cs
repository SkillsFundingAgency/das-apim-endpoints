using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor
{
    public class GetCertificateSearchRequest : IGetApiRequest
    {
        public DateTime DateOfBirth { get; }
        public string FamilyName { get; }
        public IEnumerable<long> Exclude { get; }

        public GetCertificateSearchRequest(DateTime dateOfBirth, string familyName, IEnumerable<long> exclude = null)
        {
            DateOfBirth = dateOfBirth;
            FamilyName = familyName;
            Exclude = exclude ?? Enumerable.Empty<long>();
        }

        public string GetUrl
        {
            get
            {
                var url = $"api/v1/certificates/search?dob={DateOfBirth:yyyy-MM-dd}&name={Uri.EscapeDataString(FamilyName)}";
                foreach (var uln in Exclude)
                {
                    url += $"&exclude={uln}";
                }
                return url;
            }
        }
    }
}
