using System.Linq;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;


namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp
{
    public class ProviderAddress
    {
        public const string LegalIdentifier = "L";
        public string Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public static implicit operator ProviderAddress(Provider source) =>
            new ProviderAddress
            {
                Ukprn = source.UnitedKingdomProviderReferenceNumber,
                ProviderName = source.ProviderName,
                Address1 = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.Address1,
                Address2 = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.Address2,
                Address3 = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.Address3,
                Address4 = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.Address4,
                Town = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.Town,
                Postcode = source.ProviderContacts.FirstOrDefault(x => x.ContactType == LegalIdentifier)?.ContactAddress?.PostCode

            };
    }
}

