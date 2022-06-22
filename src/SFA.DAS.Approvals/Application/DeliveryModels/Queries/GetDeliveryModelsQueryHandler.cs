using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
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
            DeliveryModels = new List<string> { "Regular" }
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
                return await GetValidProviderCoursesDeliveryModels(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting Provider Courses Delivery Models for Provider {providerId} and course {trainingCode}", request.ProviderId, request.TrainingCode);
                return DefaultDeliveryModels;
            }
        }

        private async Task<GetDeliveryModelsQueryResult> GetValidProviderCoursesDeliveryModels(GetDeliveryModelsQuery request)
        {
            _logger.LogInformation("Requesting DeliveryModels for Provider {ProviderId} and Course { TrainingCode}", request.ProviderId, request.TrainingCode);
            var result = await _apiClient.Get<GetDeliveryModelsQueryResult>(new GetDeliveryModelsRequest(request.ProviderId, request.TrainingCode));

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

            _logger.LogInformation("Requesting ale from AccountsApiClient {LegalEntityId}", request.AccountLegalEntityId.ToString());
            var ale = await _accountsApiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

            if (ale != null)
            {
                _logger.LogInformation("Requesting fjaa agency for LegalEntityId {LegalEntityId}", ale.MaLegalEntityId);
                var agency = await _fjaaClient.Get<GetAgencyResponse>(new GetAgencyRequest((int)ale.MaLegalEntityId));

                return new GetDeliveryModelsQueryResult() { DeliveryModels = this.AssignDeliveryModels(result.DeliveryModels, agency != null) };
            }

            return new GetDeliveryModelsQueryResult() { DeliveryModels = result.DeliveryModels };
        }

        private List<string> AssignDeliveryModels(List<string> models, bool agencyExists)
        {
            bool portable = models.Contains(DeliveryModelStringTypes.PortableFlexiJob) ? true : false;

            if (agencyExists && !portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); models.Add(DeliveryModelStringTypes.FlexiJobAgency); }
            if (agencyExists && portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); models.Add(DeliveryModelStringTypes.FlexiJobAgency); }
            if (!agencyExists && portable) { models.Remove(DeliveryModelStringTypes.FlexiJobAgency); }
            if (!agencyExists && !portable) { models.Remove(DeliveryModelStringTypes.PortableFlexiJob); models.Remove(DeliveryModelStringTypes.FlexiJobAgency); }

            return models;
        }
    }
}