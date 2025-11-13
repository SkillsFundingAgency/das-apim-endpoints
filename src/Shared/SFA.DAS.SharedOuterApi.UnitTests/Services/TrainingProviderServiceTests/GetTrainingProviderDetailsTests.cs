using System.Net;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.TrainingProviderServiceTests;

[TestFixture]
public class GetTrainingProviderDetailsTests
{
    [Test, AutoData]
    public void WhenNoTrainingProviderIsFound(
        long trainingProviderId,
        [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
    {
        SetupClient(client, null, HttpStatusCode.NotFound);
        var sut = new TrainingProviderService(client.Object);
        sut.Invoking((s) => s.GetTrainingProviderDetails(trainingProviderId))
            .Should().ThrowAsync<HttpRequestContentException>()
            .WithMessage($"Training Provider Id {trainingProviderId} not found");
    }

    [Test, AutoData]
    public async Task WhenASingleTrainingProvidersIsFound(
        long trainingProviderId,
        OrganisationResponse result,
        [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
    {
        SetupClient(client, result);

        var sut = new TrainingProviderService(client.Object);
        var response = await sut.GetTrainingProviderDetails(trainingProviderId);
        TrainingProviderResponse expected = result;
        response.Should().BeEquivalentTo(expected);
    }

    [Test, AutoData]
    public void WhenAnErrorIsFound(
        long trainingProviderId,
        [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client)
    {
        SetupClient(client, null, HttpStatusCode.InternalServerError, "some internal error");
        var sut = new TrainingProviderService(client.Object);
        sut.Invoking((s) => s.GetTrainingProviderDetails(trainingProviderId))
            .Should().ThrowAsync<HttpRequestContentException>().WithMessage("some internal error");
    }

    public static void SetupClient(
        Mock<IInternalApiClient<TrainingProviderConfiguration>> client,
        OrganisationResponse response,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string error = null)
    {
        client.Setup(x => x.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>()))
            .ReturnsAsync(new ApiResponse<OrganisationResponse>(response, statusCode, error));
    }
}
