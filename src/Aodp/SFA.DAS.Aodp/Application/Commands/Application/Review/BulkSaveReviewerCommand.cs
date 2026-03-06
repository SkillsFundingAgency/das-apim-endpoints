using MediatR;

namespace SFA.DAS.AODP.Application.Commands.Application.Review
{
    public class BulkSaveReviewerCommand : IRequest<BaseMediatrResponse<BulkSaveReviewerCommandResponse>>
    {
        public List<Guid> ApplicationIds { get; set; } = new List<Guid>();
        public bool Reviewer1Set { get; set; }
        public string? Reviewer1 { get; set; }
        public bool Reviewer2Set { get; set; }
        public string? Reviewer2 { get; set; }
        public required string UserType { get; set; }
        public required string SentByName { get; set; }
        public required string SentByEmail { get; set; }
    }
}