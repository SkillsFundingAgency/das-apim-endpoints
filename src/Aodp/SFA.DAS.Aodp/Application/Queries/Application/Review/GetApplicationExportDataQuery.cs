using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    [ExcludeFromCodeCoverage]
    public class GetApplicationExportDataQuery : IRequest<BaseMediatrResponse<GetApplicationExportDataQueryResponse>>
    {
        public Guid ApplicationReviewId { get; set; }

        public GetApplicationExportDataQuery(Guid applicationReviewId)
        {
            ApplicationReviewId = applicationReviewId;
        }
    }
}
