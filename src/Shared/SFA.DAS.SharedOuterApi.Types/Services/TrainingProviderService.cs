using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Interfaces;


using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Types.Models.Roatp;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class TrainingProviderService : ITrainingProviderService
{
    private readonly IInternalApiClient<TrainingProviderConfiguration> _client;

    public TrainingProviderService(IInternalApiClient<TrainingProviderConfiguration> client) => _client = client;

    [Obsolete("Use GetProviderDetails(int ukprn) instead")]
    public async Task<TrainingProviderResponse> GetTrainingProviderDetails(long ukprn)
    {
        var organisationResponse = await _client.GetWithResponseCode<OrganisationResponse>(new GetOrganisationRequest((int)ukprn));

        if (organisationResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new HttpRequestContentException($"Training Provider Id {ukprn} not found", System.Net.HttpStatusCode.NotFound, "");
        }

        if (organisationResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new HttpRequestContentException(organisationResponse.ErrorContent, organisationResponse.StatusCode);
        }

        TrainingProviderResponse result = organisationResponse.Body;

        return result;
    }

    public async Task<ProviderDetailsModel> GetProviderDetails(int ukprn)
    {
        var organisationResponse = await _client.GetWithResponseCode<OrganisationResponse>(new GetOrganisationRequest(ukprn));

        if (organisationResponse.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

        organisationResponse.EnsureSuccessStatusCode();

        ProviderDetailsModel result = organisationResponse.Body;

        return result;
    }
}
