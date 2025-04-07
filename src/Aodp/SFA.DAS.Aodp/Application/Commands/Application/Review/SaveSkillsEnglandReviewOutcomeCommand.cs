using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveSkillsEnglandReviewOutcomeCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid ApplicationReviewId { get; set; }
        public string? Comments { get; set; }
        public bool Approved { get; set; }
        public string SentByName { get; set; }
        public string SentByEmail { get; set; }
    }
}