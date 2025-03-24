using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationFormAnswersByReviewIdQuery : IRequest<BaseMediatrResponse<GetApplicationFormAnswersByReviewIdQueryResponse>>
{
    public GetApplicationFormAnswersByReviewIdQuery(Guid applicationReviewId)
    {
        ApplicationReviewId = applicationReviewId;
    }
    public Guid ApplicationReviewId { get; set; }
}
