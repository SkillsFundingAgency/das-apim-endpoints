using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationsForReviewQuery : IRequest<BaseMediatrResponse<GetApplicationsForReviewQueryResponse>>
    {
        [AllowedCharacters(TextCharacterProfile.Title)]
        public string? ApplicationSearch { get; set; }
        [AllowedCharacters(TextCharacterProfile.FreeText)]
        public string? AwardingOrganisationSearch { get; set; }
        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string? ReviewerSearch { get; set; }
        public bool UnassignedOnly { get; set; }
        public List<string>? ApplicationStatuses { get; set; } = new();
        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string ReviewUser { get; set; }


        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public bool ApplicationsWithNewMessages { get; set; }
    }
}

