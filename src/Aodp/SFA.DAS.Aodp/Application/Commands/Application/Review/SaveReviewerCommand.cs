using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveReviewerCommand : IRequest<BaseMediatrResponse<SaveReviewerCommandResponse>>
    {
        public Guid ApplicationId { get; set; }
        public required string ReviewerFieldName { get; set; }
        public string? ReviewerValue { get; set; }
        public required string UserType { get; set; }
        public required string SentByName { get; set; }
        public required string SentByEmail { get; set; }
    }
}