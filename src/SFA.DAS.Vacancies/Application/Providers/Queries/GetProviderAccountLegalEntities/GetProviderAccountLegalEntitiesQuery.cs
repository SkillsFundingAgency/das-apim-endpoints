using MediatR;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQuery : IRequest<GetProviderAccountLegalEntitiesResponse>
    {
        public int Ukprn { get ; set ; }
    }
}