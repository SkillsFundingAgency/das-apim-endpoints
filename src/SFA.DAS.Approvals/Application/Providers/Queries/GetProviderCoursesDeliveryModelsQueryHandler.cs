using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelsQueryHandler : IRequestHandler<GetProviderCoursesDeliveryModelQuery, GetProviderCourseDeliveryModelsResponse>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly ILogger<GetProviderCoursesDeliveryModelsQueryHandler> _logger;

        public GetProviderCoursesDeliveryModelsQueryHandler(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, ILogger<GetProviderCoursesDeliveryModelsQueryHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        public async Task<GetProviderCourseDeliveryModelsResponse> Handle(GetProviderCoursesDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requesting DelivertyModels for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
            var result = await _apiClient.Get<GetProviderCourseDeliveryModelsResponse>(new GetProviderCoursesDeliveryModelsRequest(request.ProviderId, request.TrainingCode));

            if (result == null)
            {
                _logger.LogInformation("No information found Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
                return new GetProviderCourseDeliveryModelsResponse
                {
                    DeliveryModels = new List<string> {"Regular"}
                };
            }

            return result;
        }
    }
}