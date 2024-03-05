using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.Application.Authorization.Queries;

public record GetCohortAccessQuery(Party Party, long PartyId, long CohortId): IRequest<bool>
{
    
}