using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeship;

public class GetApprenticeshipQuery : IRequest<GetApprenticeshipQueryResult>
{
    public long ApprenticeshipId { get; set; }
}