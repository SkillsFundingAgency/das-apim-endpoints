using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetQfauFeedbackForApplicationReviewConfirmationQuery : IRequest<BaseMediatrResponse<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse>>
    {
        public Guid ApplicationReviewId { get; set; }

        public GetQfauFeedbackForApplicationReviewConfirmationQuery(Guid applicationReviewId)
        {
            ApplicationReviewId = applicationReviewId;
        }
    }
}

