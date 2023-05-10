using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQuery : IRequest<GetMyApprenticeshipQueryResult>
{
    public Guid ApprenticeId { get; init; }
}