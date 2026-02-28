using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Qualifications;

[ExcludeFromCodeCoverage]
public class BulkUpdateQualificationStatusCommandResponse
{
    public Guid ProcessStatusId { get; init; }
    public string ProcessStatusName { get; init; } = default!;
    public int RequestedCount { get; init; }
    public int UpdatedCount { get; init; }
    public int ErrorCount { get; init; }
    public IReadOnlyCollection<BulkQualificationErrorDTO> Errors { get; init; }
        = Array.Empty<BulkQualificationErrorDTO>();
}

[ExcludeFromCodeCoverage]
public class BulkQualificationErrorDTO
{
    public Guid QualificationId { get; init; }
    public string Qan { get; init; } = default!;
    public string Title { get; init; } = default!;
    public BulkQualificationErrorType ErrorType { get; init; }
}

public enum BulkQualificationErrorType
{
    Missing = 1,
    StatusUpdateFailed = 2,
    HistoryFailed = 3
}