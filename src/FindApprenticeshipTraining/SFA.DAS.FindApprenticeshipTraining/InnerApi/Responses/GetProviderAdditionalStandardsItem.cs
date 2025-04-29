namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderAdditionalStandardsItem
    {
        public int LarsCode { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public string ApprovalBody { get; set; }
    }
}