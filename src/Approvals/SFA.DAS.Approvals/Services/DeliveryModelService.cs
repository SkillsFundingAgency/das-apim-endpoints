using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services
{
    public interface IDeliveryModelService
    {
        /// <summary>
        /// Gets valid delivery model options for given Draft Apprenticeship details (pre approval)
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="trainingCode"></param>
        /// <param name="accountLegalEntityId"></param>
        /// <param name="continuationOfId"></param>
        /// <returns></returns>
        Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId, long? continuationOfId = null);
        /// <summary>
        /// Gets valid delivery model options for an Apprenticeship (post approval)
        /// </summary>
        /// <param name="apprenticeship"></param>
        /// <returns></returns>
        Task<List<string>> GetDeliveryModels(GetApprenticeshipResponse apprenticeship);
    }

    public class DeliveryModelService : IDeliveryModelService
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILogger<DeliveryModelService> _logger;
        private readonly IFjaaService _fjaaService;

        public DeliveryModelService(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, ILogger<DeliveryModelService> logger, IFjaaService fjaaService)
        {
            _apiClient = apiClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _logger = logger;
            _fjaaService = fjaaService;
        }

        public async Task<List<string>> GetDeliveryModels(long providerId, string trainingCode, long accountLegalEntityId, long? continuationOfId = null)
        {
            var isOnPortableFlexiJobTask = IsApprenticeshipOnPortableFlexiJob(continuationOfId);
            var courseDeliveryModelsTask = GetCourseDeliveryModels(providerId, trainingCode);
            var isOnRegisterTask = _fjaaService.IsAccountLegalEntityOnFjaaRegister(accountLegalEntityId);

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

        public async Task<List<string>> GetDeliveryModels(GetApprenticeshipResponse apprenticeship)
        {
            var result = await GetDeliveryModels(apprenticeship.ProviderId, apprenticeship.CourseCode,
                apprenticeship.AccountLegalEntityId, apprenticeship.ContinuationOfId);

            if (apprenticeship.DeliveryModel == DeliveryModelStringTypes.FlexiJobAgency && !result.Contains(apprenticeship.DeliveryModel))
            {
                result.Add(DeliveryModelStringTypes.FlexiJobAgency);
            }

            return result;
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
