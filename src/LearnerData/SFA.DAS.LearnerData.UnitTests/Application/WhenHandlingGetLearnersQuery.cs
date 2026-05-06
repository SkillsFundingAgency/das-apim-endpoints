using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application;


[TestFixture]
public class WhenHandlingGetLearnersQuery
{

    [Test, MoqAutoData]
    public async Task Then_Gets_Learners(
        GetLearnersQuery query,
        GetLearnersQueryResult expectedResult,
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient)
    {
        // Arrange
        var handler = new GetLearnersQueryHandler(learningApiClient.Object);
        learningApiClient
            .Setup(x => x.Get<GetLearnersQueryResult>(It.IsAny<GetAllLearningsRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);

        learningApiClient.Verify(x => x.Get<GetLearnersQueryResult>(
            It.IsAny<GetAllLearningsRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void Handle_WhenApiClientThrows_ThrowsException(
        GetLearnersQuery query,
        GetLearnersQueryResult expectedResult,
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient)
    {
        // Arrange
        var handler = new GetLearnersQueryHandler(learningApiClient.Object);

        learningApiClient
            .Setup(x => x.Get<GetLearnersQueryResult>(It.IsAny<GetAllLearningsRequest>()))
            .ThrowsAsync(new Exception("API failure"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None))
              ?.Message.Should().Be("API failure");
    }
}
