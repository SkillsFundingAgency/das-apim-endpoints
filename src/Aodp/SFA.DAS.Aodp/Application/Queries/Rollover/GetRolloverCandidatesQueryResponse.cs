namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesQueryResponse
    {
        public IEnumerable<RolloverCandidate> RolloverCandidates { get; set; } = new List<RolloverCandidate>();
    }

    public class RolloverCandidate
    {
        public Guid Id { get; set; }

        public Guid QualificationVersionId { get; set; }

        public Guid FundingOfferId { get; set; }

        public bool IsActive { get; set; }

        public string Qan { get; init; }

        public string Title { get; init; }

        public string AwardingOrganisation { get; init; }

        public string FundingOffer { get; init; }

        public DateTime? FundingApprovalEndDate { get; init; }
    }
}