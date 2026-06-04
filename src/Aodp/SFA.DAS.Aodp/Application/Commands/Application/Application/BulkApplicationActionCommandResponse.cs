using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    [ExcludeFromCodeCoverage]
    public class BulkApplicationActionCommandResponse
    {
        public int RequestedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int ErrorCount { get; set; }

        public IReadOnlyCollection<BulkApplicationActionErrorDto> Errors { get; set; }
            = Array.Empty<BulkApplicationActionErrorDto>();
    }

    [ExcludeFromCodeCoverage]
    public class BulkApplicationActionErrorDto
    {
        public Guid ApplicationId { get; init; }
        public Guid ApplicationReviewId { get; init; }
        public int ReferenceNumber { get; init; } = default!;
        public string? Qan { get; set; }
        public string? Title { get; set; }
        public string? AwardingOrganisation { get; set; }
        public BulkApplicationActionErrorType ErrorType { get; init; }
    }

    public enum BulkApplicationActionErrorType
    {
        InvalidAction = 1,
        UpdateFailed = 2
    }
}
