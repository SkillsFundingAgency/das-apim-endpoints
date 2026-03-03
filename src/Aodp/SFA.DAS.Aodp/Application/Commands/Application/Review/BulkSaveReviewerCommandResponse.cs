using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AODP.Application.Commands.Application.Review
{
    [ExcludeFromCodeCoverage]
    public class BulkSaveReviewerCommandResponse
    {
        public int RequestedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int ErrorCount { get; set; }

        public IReadOnlyCollection<BulkReviewerErrorDto> Errors { get; init; }
            = Array.Empty<BulkReviewerErrorDto>();
    }

    [ExcludeFromCodeCoverage]
    public class BulkReviewerErrorDto
    {
        public Guid ApplicationId { get; init; }
        public int ReferenceNumber { get; init; } = default!;
        public string? Qan { get; set; }

        //Might need this
        //public string? QualificationTitle { get; set; }
        public BulkReviewerErrorType ErrorType { get; init; }
    }

    public enum BulkReviewerErrorType
    {
        Missing = 1,                 // Application not found
        Conflict = 2,                // Reviewer1 == Reviewer2
        MessageFailed = 3            // Update succeeded but message logging failed
    }
}
