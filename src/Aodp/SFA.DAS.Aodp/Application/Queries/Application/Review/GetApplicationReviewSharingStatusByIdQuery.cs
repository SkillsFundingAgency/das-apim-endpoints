using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationReviewSharingStatusByIdQuery : IRequest<BaseMediatrResponse<GetApplicationReviewSharingStatusByIdQueryResponse>>
    {
        public Guid ApplicationReviewId { get; set; }

        public GetApplicationReviewSharingStatusByIdQuery(Guid applicationReviewId)
        {
            ApplicationReviewId = applicationReviewId;
        }
    }

}

