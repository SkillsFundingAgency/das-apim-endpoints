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
        public string? QualificationNumber { get; init; }
        public Guid FundingOfferId { get; set; }
        public string? FundingOfferName { get; init; }
        public string? AcademicYear { get; set; }
    }
}