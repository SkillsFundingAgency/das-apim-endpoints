using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer
{
    public class GetConfirmEmployerQuery : IRequest<GetConfirmEmployerQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
}
