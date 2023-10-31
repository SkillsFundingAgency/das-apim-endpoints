using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQuery : IRequest<GetManageApprenticeshipDetailsQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}
