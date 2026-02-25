using SFA.DAS.LearnerData.Application.GetShortCourseLearners;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application;

[TestFixture]
public class WhenHandlingGetShortCourseLearnersQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Learners(
        GetShortCourseLearnersQuery query,
        GetShortCourseLearnersQueryResult expectedResult,
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient)
    {
        // Arrange
        var handler = new GetShortCourseLearnersQueryHandler(learningApiClient.Object);
        learningApiClient
            .Setup(x => x.Get<GetShortCourseLearnersQueryResult>(It.IsAny<GetAllShortCourseLearningsRequest>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);

        learningApiClient.Verify(x => x.Get<GetShortCourseLearnersQueryResult>(
            It.IsAny<GetAllShortCourseLearningsRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void Handle_WhenApiClientThrows_ThrowsException(
        GetShortCourseLearnersQuery query,
        GetShortCourseLearnersQueryResult expectedResult,
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient)
    {
        // Arrange
        var handler = new GetShortCourseLearnersQueryHandler(learningApiClient.Object);

        learningApiClient
            .Setup(x => x.Get<GetShortCourseLearnersQueryResult>(It.IsAny<GetAllShortCourseLearningsRequest>()))
            .ThrowsAsync(new Exception("API failure"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None))
              ?.Message.Should().Be("API failure");
    }
}