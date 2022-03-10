using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelsQueryHandler : IRequestHandler<GetProviderCoursesDeliveryModelQuery, GetProviderCourseDeliveryModelsResponse>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;

        public GetProviderCoursesDeliveryModelsQueryHandler(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProviderCourseDeliveryModelsResponse> Handle(GetProviderCoursesDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetProviderCourseDeliveryModelsResponse>(new GetProviderCoursesDeliveryModelsRequest(request.ProviderId, request.TrainingCode));

            if (result == null)
            {
                return new GetProviderCourseDeliveryModelsResponse
                {
                    DeliveryModels = new List<string> {"Regular"}
                };
            }

            return result;
        }
    }
}