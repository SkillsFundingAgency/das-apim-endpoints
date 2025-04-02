using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class UpdateApplicationReviewSharingCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public string ApplicationReviewUserType { get; set; }
        public bool ShareApplication { get; set; }
        public Guid ApplicationReviewId { get; set; }
        public string UserType { get; set; }
        public string SentByName { get; set; }
        public string SentByEmail { get; set; }
    }

}
