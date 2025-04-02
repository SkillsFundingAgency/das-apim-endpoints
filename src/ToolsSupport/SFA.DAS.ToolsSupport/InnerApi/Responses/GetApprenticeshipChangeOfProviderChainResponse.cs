namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetApprenticeshipChangeOfProviderChainResponse
{
    public List<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }

    public class ChangeOfProviderLink
    {
        public long ApprenticeshipId { get; set; }
        public string ProviderName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}