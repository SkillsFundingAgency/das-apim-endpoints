using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Application.Qualifications
{
    public class SaveQualificationFundingOffersOutcomeCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid QualificationVersionId { get; set; }
        public string? Comments { get; set; }
        public bool? Approved { get; set; }
    }

}