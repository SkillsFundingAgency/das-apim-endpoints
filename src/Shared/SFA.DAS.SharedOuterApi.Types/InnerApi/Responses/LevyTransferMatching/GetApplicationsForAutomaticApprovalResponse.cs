namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;

public class GetApplicationsForAutomaticApprovalResponse
{
    public IEnumerable<Application> Applications { get; set; }

    public class Application
    {
        public int Id { get; set; }
        public long EmployerAccountId { get; set; }
        public int PledgeId { get; set; }
        public int MatchPercentage { get; set; }
        public AutomaticApprovalOption PledgeAutomaticApprovalOption { get; set; }
        public int Amount { get; set; }
        public int PledgeRemainingAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
    }
}
