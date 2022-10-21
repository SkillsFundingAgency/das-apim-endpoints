namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetStandardResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public string ApprovalBody { get; set; }
        public string Route { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
    }
}
