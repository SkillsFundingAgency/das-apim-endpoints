using MediatR;

namespace SFA.DAS.VacanciesManage.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQuery : IRequest<GetProviderAccountLegalEntitiesQueryResponse>
    {
        public int Ukprn { get ; set ; }
    }
}