using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacancyQueryHandler : IRequestHandler<GetVacancyQuery, GetVacancyQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ICourseService _standardsService;
        private readonly VacanciesConfiguration _vacanciesConfiguration;

        public GetVacancyQueryHandler (IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, 
            ICourseService standardsService,
            IOptions<VacanciesConfiguration> vacanciesConfiguration)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _standardsService = standardsService;
            _vacanciesConfiguration = vacanciesConfiguration.Value;
        }
        public async Task<GetVacancyQueryResult> Handle(GetVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancyResponseTask =
                 _findApprenticeshipApiClient.Get<GetVacancyApiResponse>(
                    new GetVacancyRequest(request.VacancyReference));

            var standardsTask = _standardsService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            await Task.WhenAll(vacancyResponseTask, standardsTask);

            if (vacancyResponseTask.Result == null)
            {
                return new GetVacancyQueryResult
                {
                    Vacancy = null
                };
            }
            
            vacancyResponseTask.Result.VacancyUrl = vacancyResponseTask.Result.VacancyUrl = $"{_vacanciesConfiguration.FindAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancyResponseTask.Result.VacancyReference}";

            if (vacancyResponseTask.Result.StandardLarsCode != null)
            {
                var standard = standardsTask.Result.Standards.SingleOrDefault(c =>
                    c.LarsCode.Equals(vacancyResponseTask.Result.StandardLarsCode));

                if (standard != null)
                {
                    vacancyResponseTask.Result.CourseLevel = standard.Level;
                    vacancyResponseTask.Result.CourseTitle = standard.Title;
                    vacancyResponseTask.Result.Route = standard.Route;
                }
            }
            
            return new GetVacancyQueryResult
            {
                Vacancy = vacancyResponseTask.Result
            };
        }
    }
}