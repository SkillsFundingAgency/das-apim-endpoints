using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics
{
    public record GetAllVacanciesInMetricsQueryHandler : IRequestHandler<GetAllVacanciesInMetricsQuery, GetAllVacanciesInMetricsQueryResult>
    {
        private readonly IBusinessMetricsApiClient<BusinessMetricsConfiguration> _apiClient;

        public GetAllVacanciesInMetricsQueryHandler(IBusinessMetricsApiClient<BusinessMetricsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAllVacanciesInMetricsQueryResult> Handle(GetAllVacanciesInMetricsQuery request, CancellationToken cancellationToken)
        {
            var outerApiResponse = await _apiClient.Get<GetAllVacanciesResponse>(new GetAllVacanciesRequest(request.StartDate, request.EndDate));
            
            if (outerApiResponse?.Vacancies is null)
            {
                return new GetAllVacanciesInMetricsQueryResult();
            }

            return new GetAllVacanciesInMetricsQueryResult
            {
                Vacancies = outerApiResponse.Vacancies
            };
        }
    }
}