namespace SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses
{
    public class GetProviderStandardsResponse
    {
        public int LarsCode { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }

        public string CourseNameWithLevel => $"{CourseName}, Level: {Level}";
    }
}
