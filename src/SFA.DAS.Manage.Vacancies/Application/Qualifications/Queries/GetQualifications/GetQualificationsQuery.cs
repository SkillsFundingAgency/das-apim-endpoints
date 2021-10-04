using MediatR;

namespace SFA.DAS.Vacancies.Manage.Application.Qualifications.Queries.GetQualifications
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResponse>
    {
        public int Ukprn { get ; set ; }
    }
}