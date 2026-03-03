using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    [ExcludeFromCodeCoverage]
    public class BulkApplicationMessageCommandResponse
    {
        public int RequestedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int ErrorCount { get; set; }

        public IReadOnlyCollection<BulkApplicationMessageErrorDto> Errors { get; set; }
            = Array.Empty<BulkApplicationMessageErrorDto>();
    }

    [ExcludeFromCodeCoverage]
    public class BulkApplicationMessageErrorDto
    {
        public Guid ApplicationId { get; init; }
        public int ReferenceNumber { get; init; } = default!;
        public string? Qan { get; set; }
        public BulkApplicationMessageErrorType ErrorType { get; init; }
    }

    public enum BulkApplicationMessageErrorType
    {
        General = 1            
    }
}
