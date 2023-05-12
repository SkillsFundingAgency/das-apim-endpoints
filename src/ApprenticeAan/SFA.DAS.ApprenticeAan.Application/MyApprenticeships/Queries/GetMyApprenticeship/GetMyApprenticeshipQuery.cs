using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public class GetMyApprenticeshipQuery : IRequest<MyApprenticeship?>
{
    public Guid ApprenticeId { get; init; }
}