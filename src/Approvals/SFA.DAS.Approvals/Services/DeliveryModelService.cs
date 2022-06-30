using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IDeliveryModelService
    {
        Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId);
    }

    public class DeliveryModelService : IDeliveryModelService
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILogger<DeliveryModelService> _logger;
        private readonly FeatureToggles _featureToggles;

        public DeliveryModelService(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, IFjaaApiClient<FjaaApiConfiguration> fjaaClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, ILogger<DeliveryModelService> logger, FeatureToggles featureToggles)
        {
            _apiClient = apiClient;
            _fjaaClient = fjaaClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        public async Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId)
        {
            var courseDeliveryModelsTask = GetCourseDeliveryModels(providerId, trainingCode);
            var isOnRegisterTask = IsLegalEntityOnFjaaRegister(accountLegalEntityId);

            await Task.WhenAll(courseDeliveryModelsTask, isOnRegisterTask);

            var courseDeliveryModels = courseDeliveryModelsTask.Result;
            var isOnRegister = isOnRegisterTask.Result;

            if (!isOnRegister)
            {
                return courseDeliveryModels;
            }

            courseDeliveryModels.Add(DeliveryModelStringTypes.FlexiJobAgency);
            courseDeliveryModels.Remove(DeliveryModelStringTypes.PortableFlexiJob);

            return courseDeliveryModels;
        }

        private async Task<List<string>> GetCourseDeliveryModels(long providerId, string trainingCode)
        {
            _logger.LogInformation($"Requesting DeliveryModels for Provider {providerId} and course { trainingCode}");
            var result = await _apiClient.Get<GetDeliveryModelsResponse>(new GetDeliveryModelsRequest(providerId, trainingCode));

            if (result?.DeliveryModels == null || !result.DeliveryModels.Any())
            {
                _logger.LogInformation($"No information found for Provider {providerId} and Course {trainingCode}");
                return new List<string> { DeliveryModelStringTypes.Regular };
            }

            return result.DeliveryModels;
        }

        private async Task<bool> IsLegalEntityOnFjaaRegister(long accountLegalEntityId)
        {
            if (accountLegalEntityId == 0 || !_featureToggles.ApprovalsFeatureToggleFjaaEnabled) return false;

            _logger.LogInformation($"Requesting AccountLegalEntity {accountLegalEntityId} from Commitments v2 Api");
            var accountLegalEntity = await _commitmentsV2ApiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(accountLegalEntityId));

            _logger.LogInformation($"Requesting fjaa agency for LegalEntityId {accountLegalEntity.MaLegalEntityId}");
            var agencyRequest = await _fjaaClient.GetWithResponseCode<GetAgencyResponse>(new GetAgencyRequest(accountLegalEntity.MaLegalEntityId));

            if (agencyRequest.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            agencyRequest.EnsureSuccessStatusCode();
            return true;
        }
    }
}
