using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.AODP.Application.Commands.Application.Review
{
    public class BulkSaveReviewerCommand : IRequest<BaseMediatrResponse<BulkSaveReviewerCommandResponse>>
    {
        public List<Guid> ApplicationReviewIds { get; set; } = new List<Guid>();
        public bool Reviewer1Set { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string? Reviewer1 { get; set; }

        public bool Reviewer2Set { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string? Reviewer2 { get; set; }

        [UserType]
        public required string UserType { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public required string SentByName { get; set; }

        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public required string SentByEmail { get; set; }
    }
}