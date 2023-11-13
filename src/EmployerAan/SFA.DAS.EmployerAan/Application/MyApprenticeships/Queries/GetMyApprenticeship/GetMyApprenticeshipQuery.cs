using MediatR;

namespace SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public record GetMyApprenticeshipQuery(Guid ApprenticeId) : IRequest<GetMyApprenticeshipQueryResult?>;