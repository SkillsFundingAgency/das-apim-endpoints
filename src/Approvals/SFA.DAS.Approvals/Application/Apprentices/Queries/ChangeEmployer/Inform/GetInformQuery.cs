using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.Inform
{
    public class GetInformQuery : IRequest<GetInformQueryResult>
    {
        public long ApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
    }
}