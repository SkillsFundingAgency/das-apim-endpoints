using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ApprenticeData
{
    public class GetChangeOfEmployerApprenticeDataQuery : IRequest<GetChangeOfEmployerApprenticeDataQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
}
