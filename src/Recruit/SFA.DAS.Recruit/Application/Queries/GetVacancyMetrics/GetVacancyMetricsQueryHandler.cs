using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics
{
    public record GetVacancyMetricsQueryHandler : IRequestHandler<GetVacancyMetricsQuery, GetVacancyMetricsQueryResult>
    {
        private readonly IBusinessMetricsApiClient<BusinessMetricsConfiguration> _apiClient;

        public GetVacancyMetricsQueryHandler(IBusinessMetricsApiClient<BusinessMetricsConfiguration> apiClient) => _apiClient = apiClient;

        public async Task<GetVacancyMetricsQueryResult> Handle(GetVacancyMetricsQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _apiClient.Get<GetVacancyMetricsResponse>(new GetVacancyMetricsRequest(request.StartDate, request.EndDate));
            
            return apiResponse ?? new GetVacancyMetricsQueryResult();
        }
    }
}