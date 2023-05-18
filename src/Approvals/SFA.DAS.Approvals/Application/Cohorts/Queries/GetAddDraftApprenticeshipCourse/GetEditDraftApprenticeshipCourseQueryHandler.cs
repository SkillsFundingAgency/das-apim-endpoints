using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipCourse
{
    public class GetAddDraftApprenticeshipCourseQueryHandler : IRequestHandler<GetAddDraftApprenticeshipCourseQuery, GetAddDraftApprenticeshipCourseQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetAddDraftApprenticeshipCourseQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetAddDraftApprenticeshipCourseQueryResult> Handle(GetAddDraftApprenticeshipCourseQuery request, CancellationToken cancellationToken)
        {
            var providerId = _serviceParameters.CallingParty == Party.Employer
                ? request.ProviderId.Value
                : _serviceParameters.CallingPartyId;

            var providerResponseTask = _apiClient.GetWithResponseCode<GetProviderResponse>(new GetProviderRequest(providerId));
            var accountLegalEntityResponseTask = _apiClient.GetWithResponseCode<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

            await Task.WhenAll(providerResponseTask, accountLegalEntityResponseTask);

            if (providerResponseTask.Result.StatusCode == HttpStatusCode.NotFound || accountLegalEntityResponseTask.Result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            providerResponseTask.Result.EnsureSuccessStatusCode();
            accountLegalEntityResponseTask.Result.EnsureSuccessStatusCode();

            var provider = providerResponseTask.Result.Body;
            var accountLegalEntity = accountLegalEntityResponseTask.Result.Body;

            if (provider == null || accountLegalEntity == null) return null;

            var providerStandardsData = await _providerStandardsService.GetStandardsData(_serviceParameters.CallingPartyId);

            return new GetAddDraftApprenticeshipCourseQueryResult
            {
                EmployerName = accountLegalEntity.LegalEntityName,
                ProviderName = provider.Name,
                IsMainProvider = providerStandardsData.IsMainProvider,
                Standards = providerStandardsData.Standards.Select(x =>
                    new GetAddDraftApprenticeshipCourseQueryResult.Standard
                        { CourseCode = x.CourseCode, Name = x.Name })
            };
        }
    }
}