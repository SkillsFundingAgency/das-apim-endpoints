using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DeliveryModels.Queries
{
    public class GetDeliveryModelsQueryHandler : IRequestHandler<GetDeliveryModelsQuery, GetDeliveryModelsQueryResult>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _apiClient;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly ILogger<GetDeliveryModelsQueryHandler> _logger;

        private static GetDeliveryModelsQueryResult DefaultDeliveryModels => new GetDeliveryModelsQueryResult
        {
            DeliveryModels = new List<string> { DeliveryModelStringTypes.Regular }
        };

        public GetDeliveryModelsQueryHandler(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> apiClient, ILogger<GetDeliveryModelsQueryHandler> logger, IFjaaApiClient<FjaaApiConfiguration> fjaaClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _apiClient = apiClient;
            _fjaaClient = fjaaClient;
            _logger = logger;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetDeliveryModelsQueryResult> Handle(GetDeliveryModelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await GetValidDeliveryModels(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }
        }

        private async Task<GetDeliveryModelsQueryResult> GetValidDeliveryModels(GetDeliveryModelsQuery request)
        {
            _logger.LogInformation("Requesting DeliveryModels for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
            var result = await _apiClient.Get<GetDeliveryModelsResponse>(new GetDeliveryModelsRequest(request.ProviderId, request.TrainingCode));

            var isOnRegister = await IsLegalEntityOnFjaaRegister(request.AccountLegalEntityId);

            if (isOnRegister)
            {
                if (result.DeliveryModels.Contains(DeliveryModelStringTypes.PortableFlexiJob))
                {
                    result.DeliveryModels.Remove(DeliveryModelStringTypes.PortableFlexiJob);
                }

                result.DeliveryModels.Add(DeliveryModelStringTypes.FlexiJobAgency);

                return new GetDeliveryModelsQueryResult() { DeliveryModels = result.DeliveryModels };
            }

            if (result == null || result?.DeliveryModels == null || result?.DeliveryModels.Count() == 0)
            {
                _logger.LogInformation("No information found for Provider {ProviderId} and Course {TrainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }

            return new GetDeliveryModelsQueryResult() { DeliveryModels = result.DeliveryModels };
        }

        private async Task<bool> IsLegalEntityOnFjaaRegister(long accountLegalEntityId)
        {
            _logger.LogInformation("Requesting ale from AccountsApiClient {LegalEntityId}", accountLegalEntityId);
            var accountLegalEntity = await _accountsApiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(accountLegalEntityId));

            _logger.LogInformation("Requesting fjaa agency for LegalEntityId {LegalEntityId}", accountLegalEntity.MaLegalEntityId);
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