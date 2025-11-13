using System;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Roatp;

namespace SFA.DAS.SharedOuterApi.Services;

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
        var organisationResponse = await _client.GetWithResponseCode<OrganisationResponse>(new GetOrganisationRequest((int)ukprn));

        if (organisationResponse.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

        organisationResponse.EnsureSuccessStatusCode();

        ProviderDetailsModel result = organisationResponse.Body;

        return result;
    }
}
