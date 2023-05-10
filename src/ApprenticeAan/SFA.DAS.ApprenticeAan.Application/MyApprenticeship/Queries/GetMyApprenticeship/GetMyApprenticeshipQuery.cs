using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQuery : IRequest<MyApprenticeship?>
{
    public Guid ApprenticeId { get; init; }
}