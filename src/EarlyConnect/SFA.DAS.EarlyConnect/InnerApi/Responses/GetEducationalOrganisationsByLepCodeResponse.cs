namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetEducationalOrganisationsByLepCodeResponse
    {
        public int TotalCount { get; set; }
        public ICollection<EducationalOrganisationData>? EducationalOrganisations { get; set; }
    }
    public class EducationalOrganisationData
    {
        public string? Name { get; set; }
        public string? AddressLine1 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? PostCode { get; set; }
        public string? URN { get; set; }
    }
}


