using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class ValidateFundingExtensionCandidatesCommandResponse
    {
        public bool IsValid { get; set; }

        public int TotalCandidates { get; set; }

        public int FailedCandidateCount { get; set; }

        public List<CandidateValidationResult> Candidates { get; set; } = new();

        public List<ValidationFailureGroup> FailureSummary { get; set; } = new();
    }

    [ExcludeFromCodeCoverage]
    public class CandidateValidationResult
    {
        public int RowNumber { get; set; }
        public string Qan { get; set; }

        public bool IsValid { get; set; }

        public string FundingStreamName { get; set; }

        public string RolloverStatus { get; set; }

        public string ExclusionReason { get; set; }

        public DateTime ProposedFundingEndDate { get; set; }

        public List<ValidationError> Errors { get; set; } = new();
    }

    [ExcludeFromCodeCoverage]
    public class ValidationFailureGroup
    {
        public string Field { get; set; }   

        public string Message { get; set; } 

        public int Count { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }

}