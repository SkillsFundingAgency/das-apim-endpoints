using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;


namespace SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;

public record GetCohortAccessQuery(Party Party, long PartyId, long CohortId) : IRequest<bool>
{
}