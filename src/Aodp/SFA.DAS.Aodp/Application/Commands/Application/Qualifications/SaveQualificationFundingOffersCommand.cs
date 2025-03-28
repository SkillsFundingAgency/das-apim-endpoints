using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveQualificationFundingOffersCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid QualificationVersionId { get; set; }
        public List<Guid> SelectedOfferIds { get; set; } = new();
        public Guid QualificationId { get; set; }
        public string? QualificationReference { get; set; }
        public Guid ActionTypeId { get; set; }
        public string? UserDisplayName { get; set; }
    }
}