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
    public class GetTraineeshipVacancyQueryHandler : IRequestHandler<GetTraineeshipVacancyQuery, GetTraineeshipVacancyQueryResult>
    {
        private readonly IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> _findTraineeshipApiClient;
        private readonly VacanciesConfiguration _vacanciesConfiguration;

        public GetTraineeshipVacancyQueryHandler(IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> findTraineeshipApiClient, 
            ICourseService standardsService,
            IOptions<VacanciesConfiguration> vacanciesConfiguration)
        {
            _findTraineeshipApiClient = findTraineeshipApiClient;
            _vacanciesConfiguration = vacanciesConfiguration.Value;
        }
        public async Task<GetTraineeshipVacancyQueryResult> Handle(GetTraineeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancyResponseTask =
                 _findTraineeshipApiClient.Get<GetTraineeshipVacancyApiResponse>(
                    new GetTraineeshipVacancyRequest(request.VacancyReference));

            await Task.WhenAll(vacancyResponseTask);

            if (vacancyResponseTask.Result == null)
            {
                return new GetTraineeshipVacancyQueryResult
                {
                    Vacancy = null
                };
            }
            
            vacancyResponseTask.Result.VacancyUrl = vacancyResponseTask.Result.VacancyUrl = $"{_vacanciesConfiguration.FindATraineeshipBaseUrl}/traineeship/reference/{vacancyResponseTask.Result.VacancyReference}";
          
            return new GetTraineeshipVacancyQueryResult
            {
                Vacancy = vacancyResponseTask.Result
            };
        }
    }
}