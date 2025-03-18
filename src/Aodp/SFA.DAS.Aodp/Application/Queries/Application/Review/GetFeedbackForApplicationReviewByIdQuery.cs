using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetFeedbackForApplicationReviewByIdQuery : IRequest<BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>>
    {
        public Guid ApplicationReviewId { get; set; }
        public string UserType { get; set; }

        public GetFeedbackForApplicationReviewByIdQuery(Guid applicationReviewId, string userType)
        {
            ApplicationReviewId = applicationReviewId;
            UserType = userType;
        }
    }
}

