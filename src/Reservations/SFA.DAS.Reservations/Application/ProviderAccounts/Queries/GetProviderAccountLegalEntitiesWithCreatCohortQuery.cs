using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreatCohortQuery(int Ukprn) : IRequest<GetProviderAccountLegalEntitiesWithCreateCohortResult>
{
    
}