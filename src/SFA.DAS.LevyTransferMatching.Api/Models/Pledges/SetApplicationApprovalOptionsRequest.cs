namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class SetApplicationApprovalOptionsRequest
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public bool AutomaticApproval { get; set; }
    }
}
