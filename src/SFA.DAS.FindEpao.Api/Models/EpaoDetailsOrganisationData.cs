using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class EpaoDetailsOrganisationData
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteLink { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }

        public static implicit operator EpaoDetailsOrganisationData(GetEpaoOrganisationData source)
        {
            return new EpaoDetailsOrganisationData
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                WebsiteLink = source.WebsiteLink,
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                Address4 = source.Address4,
                Postcode = source.Postcode
            };
        }
    }
}