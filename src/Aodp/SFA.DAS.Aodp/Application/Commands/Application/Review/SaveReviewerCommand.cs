using MediatR;
using SFA.DAS.AODP.Application.Commands.Application.Review;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewerCommand : IRequest<BaseMediatrResponse<SaveReviewerCommandResponse>>
    {
        public Guid ApplicationId { get; set; }
        public string ReviewerFieldName { get; set; }
        public string? ReviewerValue { get; set; }
        public string UserType { get; set; }
        public string SentByName { get; set; }
        public string SentByEmail { get; set; }
    }
}