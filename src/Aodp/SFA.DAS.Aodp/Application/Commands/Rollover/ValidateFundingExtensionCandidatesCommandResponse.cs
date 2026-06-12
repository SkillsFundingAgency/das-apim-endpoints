using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class ValidateFundingExtensionCandidatesCommandResponse
    {
        public bool IsValid { get; set; }

        public int TotalCandidates { get; set; }

        public int FailedCandidateCount { get; set; }

        public List<ValidationFailureGroup> FailureSummary { get; set; } = new();
        public byte[]? ValidatedCandidateFile { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ValidationFailureGroup
    {
        public string Field { get; set; }   

        public string Message { get; set; } 

        public int Count { get; set; }
    }
}