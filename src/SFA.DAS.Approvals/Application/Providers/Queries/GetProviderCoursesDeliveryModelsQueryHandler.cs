using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Fjaa.Constants;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelsQueryHandler : IRequestHandler<GetProviderCoursesDeliveryModelQuery, GetProviderCourseDeliveryModelsResponse>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly ILogger<GetProviderCoursesDeliveryModelsQueryHandler> _logger;

        private static GetProviderCourseDeliveryModelsResponse DefaultDeliveryModels => new GetProviderCourseDeliveryModelsResponse
        {
            DeliveryModels = new List<string> { "Regular" }
        };

        public GetProviderCoursesDeliveryModelsQueryHandler(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, ILogger<GetProviderCoursesDeliveryModelsQueryHandler> logger, IFjaaApiClient<FjaaApiConfiguration> fjaaClient)
        {
            _apiClient = apiClient;
            _fjaaClient = fjaaClient;
            _logger = logger;
        }

        public async Task<GetProviderCourseDeliveryModelsResponse> Handle(GetProviderCoursesDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await GetValidProviderCoursesDeliveryModels(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }
        }

        private async Task<GetProviderCourseDeliveryModelsResponse> GetValidProviderCoursesDeliveryModels(GetProviderCoursesDeliveryModelQuery request)
        {
            _logger.LogInformation("Requesting DeliveryModels for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
            var result = await _apiClient.Get<GetProviderCourseDeliveryModelsResponse>(new GetProviderCoursesDeliveryModelsRequest(request.ProviderId, request.TrainingCode));

            _logger.LogInformation("Requesting fjaa agency for LegalEntityId {LegalEntityId}", request.LegalEntityId);
            var agency = await _fjaaClient.Get<GetAgencyResponse>(new GetAgencyRequest(request.LegalEntityId));

            if (result == null)
            {
                _logger.LogInformation("No information found for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }

            if (result.DeliveryModels == null)
            {
                _logger.LogInformation("Null information found for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }

            if (result.DeliveryModels.Count() == 0)
            {
                _logger.LogInformation("Empty information found for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }

            if (result.DeliveryModels != null)
            {
                result.DeliveryModels = this.AssignDeliveryModels(result.DeliveryModels.ToList(), agency != null);
            }

            return result;
        }

        private List<string> AssignDeliveryModels(List<string> models, bool agencyExists)
        {
            bool portable = models.Contains(DeliveryModelStringTypes.PortableFlexiJob) ? true : false;

            if (agencyExists && !portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); }
            if (agencyExists && portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); }
            if (!agencyExists && portable) { models.Remove(DeliveryModelStringTypes.FlexiJobAgency); }
            if (!agencyExists && !portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); models.Remove(DeliveryModelStringTypes.FlexiJobAgency); }

            return models;
        }
    }
}