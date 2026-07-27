using SFA.DAS.Aodp.Application.Queries.Application.Application;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    [ExcludeFromCodeCoverage]
    public class GetApplicationExportDataQueryResponse
    {
        public GetApplicationFormPreviewByIdQueryResponse ApplicationFormStructure { get; set; }

        public GetApplicationFormAnswersByReviewIdQueryResponse ApplicationFormResponse { get; set; }

        public ApplicationMetadataResponse ApplicationMetadata { get; set; } 
    }

    [ExcludeFromCodeCoverage]
    public class ApplicationMetadataResponse
    {
        public int SubmissionId { get; set; }
        public string QualificationTitle { get; set; } = string.Empty;
        public string OrganisationName { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;
        public string? Qan { get; set; } = string.Empty;
    }
}
