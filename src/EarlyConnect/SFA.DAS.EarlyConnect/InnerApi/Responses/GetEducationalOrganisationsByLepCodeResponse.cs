namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetEducationalOrganisationsByLepCodeResponse
    {
        public int TotalCount { get; set; }
        public ICollection<EducationalOrganisationData>? EducationalOrganisations { get; set; }
    }
    public class EducationalOrganisationData
    {
        public string Name { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string URN { get; set; } = string.Empty;
    }
}


