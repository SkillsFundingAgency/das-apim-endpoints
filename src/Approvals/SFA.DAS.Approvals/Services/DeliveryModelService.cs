using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IDeliveryModelService
    {
        Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId, long? continuationOfId = null);
        Task<bool> IsLegalEntityOnFjaaRegister(long accountLegalEntityId);
    }

    public class DeliveryModelService : IDeliveryModelService
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILogger<DeliveryModelService> _logger;

        public DeliveryModelService(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, IFjaaApiClient<FjaaApiConfiguration> fjaaClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, ILogger<DeliveryModelService> logger)
        {
            _apiClient = apiClient;
            _fjaaClient = fjaaClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _logger = logger;
        }

        public async Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId, long? continuationOfId = null)
        {
            var isOnPortableFlexiJobTask = IsApprenticeshipOnPortableFlexiJob(continuationOfId);
            var courseDeliveryModelsTask = GetCourseDeliveryModels(providerId, trainingCode);
            var isOnRegisterTask = IsLegalEntityOnFjaaRegister(accountLegalEntityId);

            await Task.WhenAll(courseDeliveryModelsTask, isOnRegisterTask, isOnPortableFlexiJobTask);

            var isOnPortableFlexiJob = isOnPortableFlexiJobTask.Result;
            var courseDeliveryModels = courseDeliveryModelsTask.Result;
            var isOnRegister = isOnRegisterTask.Result;

            if (isOnPortableFlexiJob)
            {
                return isOnRegister ? new List<string>() : new List<string> { DeliveryModelStringTypes.PortableFlexiJob };
            }

            if (isOnRegister || continuationOfId.HasValue)
            {
                courseDeliveryModels.Remove(DeliveryModelStringTypes.PortableFlexiJob);
            }

            if (isOnRegister)
            {
                courseDeliveryModels.Add(DeliveryModelStringTypes.FlexiJobAgency);
            }

            return courseDeliveryModels;
        }

        private async Task<List<string>> GetCourseDeliveryModels(long providerId, string trainingCode)
        {
            var deliveryModels = new List<string> { DeliveryModelStringTypes.Regular };
            if (string.IsNullOrWhiteSpace(trainingCode))
            {
                _logger.LogInformation($"Defaulting DeliveryModels to Regular because code is blank");
                return deliveryModels;
            }

            _logger.LogInformation($"Requesting DeliveryModels for Provider {providerId} and course { trainingCode}");
            var result = await _apiClient.Get<GetHasPortableFlexiJobOptionResponse>(new GetDeliveryModelsRequest(providerId, trainingCode));
         

            if (result == null)
            {
                _logger.LogInformation($"No information found for Provider {providerId} and Course {trainingCode}");
            }
            else
            {
                if (result.HasPortableFlexiJobOption)
                    deliveryModels.Add(DeliveryModelStringTypes.PortableFlexiJob);
            }

            return deliveryModels;
        }

        public async Task<bool> IsLegalEntityOnFjaaRegister(long accountLegalEntityId)
        {
            if (accountLegalEntityId == 0) return false;

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

        private async Task<bool> IsApprenticeshipOnPortableFlexiJob(long? apprenticeshipId)
        {
            if (!apprenticeshipId.HasValue)
            {
                return false;
            }

            var apprenticeship = await _commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(new GetApprenticeshipRequest(apprenticeshipId.Value));
            return apprenticeship.DeliveryModel == DeliveryModelStringTypes.PortableFlexiJob;
        }
    }
}
