using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetEpaoResponse
    {
        [JsonPropertyName("organisationId")]
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Status { get; set; }
        public int? OrganisationTypeId { get; set; }
        public string PrimaryContactName { get; set; }
        public GetEpaoOrganisationData OrganisationData { get; set; }
    }

    public class GetEpaoOrganisationData
    {
        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        private string _websiteLink;
        public string WebsiteLink
        {
            get
            {
                if (string.IsNullOrEmpty(_websiteLink))
                    return _websiteLink;
                
                if (!_websiteLink.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                    _websiteLink = "https://" + _websiteLink;
                return _websiteLink;
            }
            set => _websiteLink = value;
        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
    }
}