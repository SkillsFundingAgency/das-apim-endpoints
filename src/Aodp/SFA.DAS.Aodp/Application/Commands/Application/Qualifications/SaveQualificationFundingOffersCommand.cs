using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQualificationFundingOffersCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid QualificationVersionId { get; set; }
        public List<Guid> SelectedOfferIds { get; set; } = new();
        public Guid QualificationId { get; set; }

        [QualificationNumber]
        public string? QualificationReference { get; set; }
        public Guid ActionTypeId { get; set; }

        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string? UserDisplayName { get; set; }
    }
}