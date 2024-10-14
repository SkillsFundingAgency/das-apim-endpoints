using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode
{
    public class GetEducationalOrganisationsByLepCodeQueryHandler : IRequestHandler<GetEducationalOrganisationsByLepCodeQuery, GetEducationalOrganisationsByLepCodeResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetEducationalOrganisationsByLepCodeQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetEducationalOrganisationsByLepCodeResult> Handle(GetEducationalOrganisationsByLepCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<GetEducationalOrganisationsByLepCodeResponse>(new GetEducationalOrganisationsByLepCodeRequest(request.LepCode, request.SearchTerm, request.Page, request.PageSize));

            result.EnsureSuccessStatusCode();

            return new GetEducationalOrganisationsByLepCodeResult
            {
                EducationalOrganisations = result.Body.EducationalOrganisations,
                TotalCount = result.Body.TotalCount
            };
        }
    }
}