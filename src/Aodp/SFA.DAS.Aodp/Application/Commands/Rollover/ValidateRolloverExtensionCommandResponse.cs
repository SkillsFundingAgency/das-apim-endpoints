using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    [ExcludeFromCodeCoverage]
    public class ValidateRolloverExtensionCommandResponse
    {
        public bool IsValid { get; set; }

        public ValidationFailureSummary? ValidationFailureSummary { get; set; }

        public FundingExtensionSummary? ValidationSuccessSummary { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class ValidationFailureSummary
    {
        public int FailedCandidateCount { get; set; }
        public byte[]? ValidatedCandidateFile { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FundingExtensionSummary
    {
        public int TotalCandidatesCount { get; set; }
        public int CandidatesExtendedInUploadCount { get; set; }
        public int TotalCandidatesToBeExtendedCount { get; set; }
        public int TotalCandidatesToBeExcludedCount { get; set; }
        public int TotalCandidatesToBeReviewedCount { get; set; }

    }
}