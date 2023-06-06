using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetHasDeclaredStandards
{
    public class GetHasDeclaredStandardsQueryHandler : IRequestHandler<GetHasDeclaredStandardsQuery, GetHasDeclaredStandardsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetHasDeclaredStandardsQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetHasDeclaredStandardsQueryResult> Handle(GetHasDeclaredStandardsQuery request, CancellationToken cancellationToken)
        {
            var providerId = _serviceParameters.CallingParty == Party.Employer
                ? request.ProviderId.Value
                : _serviceParameters.CallingPartyId;

            var providerResponseResult = await _apiClient.GetWithResponseCode<GetProviderResponse>(new GetProviderRequest(providerId));

            if (providerResponseResult.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            providerResponseResult.EnsureSuccessStatusCode();

            var provider = providerResponseResult.Body;

            if (provider == null) return null;

            var providerStandardsData = await _providerStandardsService.GetStandardsData(_serviceParameters.CallingPartyId);

            return new GetHasDeclaredStandardsQueryResult
            {
                HasNoDeclaredStandards = providerStandardsData.Standards == null
            };
        }
    }
}