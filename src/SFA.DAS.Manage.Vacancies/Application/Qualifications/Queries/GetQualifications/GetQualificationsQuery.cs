using MediatR;

namespace SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResponse>
    {
        public int Ukprn { get ; set ; }
    }
}