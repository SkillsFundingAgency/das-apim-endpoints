using MediatR;

namespace SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;

public record GetCohortAccessQuery(long ProviderId, long CohortId) : IRequest<bool>
{
}