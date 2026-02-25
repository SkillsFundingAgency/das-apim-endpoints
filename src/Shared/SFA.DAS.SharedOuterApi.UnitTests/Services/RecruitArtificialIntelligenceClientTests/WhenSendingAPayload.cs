using System.Net;
using System.Threading;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.RecruitArtificialIntelligenceClientTests;

public class WhenSendingAPayload
{
    [Test, MoqAutoData]
    public async Task Then_The_Payload_Is_Sent_To_The_Correct_Endpoint(
        object payload,
        [Frozen] Mock<IInternalApiClient<RecruitArtificialIntelligenceConfiguration>> internalApiClient,
        [Greedy] RecruitArtificialIntelligenceClient sut)
    {
        // arrange
        IPostApiRequest capturedRequest = null;
        internalApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<IPostApiRequest>(), false))
            .Callback<IPostApiRequest, bool>((x, _) => capturedRequest = x)
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, null));

        // act
        await sut.SendPayloadAsync(payload, CancellationToken.None);

        // assert
        internalApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(It.IsAny<IPostApiRequest>(), false), Times.Once);
        capturedRequest.Should().NotBeNull();
        capturedRequest.PostUrl.Should().Be("api/llm");
        capturedRequest.Data.Should().Be(payload);
    }
}