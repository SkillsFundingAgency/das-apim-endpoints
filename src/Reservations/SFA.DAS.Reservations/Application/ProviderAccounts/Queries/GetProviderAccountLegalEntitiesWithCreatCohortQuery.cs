using MediatR;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreatCohortQuery(int Ukprn) : IRequest<GetProviderAccountLegalEntitiesWithCreateCohortResult>
{
    
}