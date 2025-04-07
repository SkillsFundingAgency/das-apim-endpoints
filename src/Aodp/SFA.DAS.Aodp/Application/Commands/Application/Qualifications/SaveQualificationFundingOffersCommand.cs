using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Qualifications
{
    public class SaveQualificationFundingOffersCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid QualificationVersionId { get; set; }
        public List<Guid> SelectedOfferIds { get; set; } = new();
    }
}