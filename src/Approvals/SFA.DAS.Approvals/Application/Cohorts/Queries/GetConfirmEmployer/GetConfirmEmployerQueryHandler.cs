using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployer
{
    public class GetConfirmEmployerQueryHandler : IRequestHandler<GetConfirmEmployerQuery, GetConfirmEmployerQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetConfirmEmployerQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetConfirmEmployerQueryResult> Handle(GetConfirmEmployerQuery request, CancellationToken cancellationToken)
        {
            var providerId =  _serviceParameters.CallingPartyId;

            var providerResponseResult = await _apiClient.GetWithResponseCode<GetProviderResponse>(new GetProviderRequest(providerId));

            if (providerResponseResult.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            providerResponseResult.EnsureSuccessStatusCode();

            var provider = providerResponseResult.Body;

            if (provider == null) return null;

            var providerStandardsData = await _providerStandardsService.GetStandardsData(_serviceParameters.CallingPartyId);

            return new GetConfirmEmployerQueryResult
            {
                HasNoDeclaredStandards = providerStandardsData.Standards.IsNullOrEmpty()
            };
        }
    }
}