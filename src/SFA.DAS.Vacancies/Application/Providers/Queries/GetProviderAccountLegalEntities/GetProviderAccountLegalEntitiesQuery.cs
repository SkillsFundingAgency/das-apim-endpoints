using MediatR;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQuery : IRequest<GetProviderAccountLegalEntitiesQueryResponse>
    {
        public int Ukprn { get ; set ; }
    }
}