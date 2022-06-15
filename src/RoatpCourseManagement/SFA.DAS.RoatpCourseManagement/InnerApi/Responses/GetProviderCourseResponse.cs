namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetProviderCourseResponse
    {
        public int ProviderCourseId { get; set; }
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public string ApprovalBody { get; set; }
        public string Route { get; set; }
        public string StandardInfoUrl { get; set; }
        public string ContactUsPhoneNumber { get; set; }
        public string ContactUsEmail { get; set; }
        public string ContactUsPageUrl { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
    }
}
