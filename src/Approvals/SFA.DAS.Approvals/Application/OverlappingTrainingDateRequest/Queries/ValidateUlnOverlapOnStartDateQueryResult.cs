namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class ValidateUlnOverlapOnStartDateQueryResult
    {
        public long? HasOverlapWithApprenticeshipId { get; set; }
        public bool HasStartDateOverlap { get; set; }
    }
}
