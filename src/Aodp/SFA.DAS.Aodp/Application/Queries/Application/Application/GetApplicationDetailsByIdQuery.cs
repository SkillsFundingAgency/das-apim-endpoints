using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationDetailsByIdQuery : IRequest<BaseMediatrResponse<GetApplicationDetailsByIdQueryResponse>>
{
    public GetApplicationDetailsByIdQuery(Guid applicationReviewId)
    {
        ApplicationReviewId = applicationReviewId;
    }
    public Guid ApplicationReviewId { get; set; }
}
