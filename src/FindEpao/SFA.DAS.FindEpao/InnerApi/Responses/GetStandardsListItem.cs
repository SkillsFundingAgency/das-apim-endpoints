namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public string[] StandardVersions { get; set; }
    }

}