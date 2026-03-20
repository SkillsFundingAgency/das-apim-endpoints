using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;
using SFA.DAS.LearnerData.Responses.Learning;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingGetShortCourseEarningsQuery
{
    private GetShortCourseEarningsQueryHandler _handler;
    private Mock<ILogger<GetShortCourseEarningsQueryHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<GetShortCourseEarningsQueryHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();

        _handler = new GetShortCourseEarningsQueryHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object);
    }

    [Test]
    public async Task Then_LearnerRef_Is_Mapped_From_Episode()
    {
        // Arrange
        var learningKey = Guid.NewGuid();
        const string expectedLearnerRef = "ABC123";

        var learning = new Learning
        {
            LearningKey = learningKey,
            Learner = new Learner { Uln = "1000000001", FirstName = "Test", LastName = "Learner", DateOfBirth = new DateTime(2000, 1, 1) },
            Episodes =
            [
                new Episode { CourseCode = "91", Price = 1000, IsApproved = true, LearnerRef = expectedLearnerRef }
            ]
        };

        _learningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = [learning], TotalItems = 1 });

        _earningsApiClient
            .Setup(x => x.GetWithResponseCode<GetShortCourseDataResponse>(It.IsAny<GetShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetShortCourseDataResponse>(
                new GetShortCourseDataResponse { Earnings = [] },
                System.Net.HttpStatusCode.OK, ""));

        var query = new GetShortCourseEarningsQuery(12345678, 2526, 1, page: 1, pageSize: 20);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Learners.Should().HaveCount(1);
        result.Learners[0].LearnerRef.Should().Be(expectedLearnerRef);
    }

    [Test]
    public async Task Then_LearnerRef_Is_Empty_When_No_Episodes()
    {
        // Arrange
        var learningKey = Guid.NewGuid();

        var learning = new Learning
        {
            LearningKey = learningKey,
            Learner = new Learner { Uln = "1000000001", FirstName = "Test", LastName = "Learner", DateOfBirth = new DateTime(2000, 1, 1) },
            Episodes = []
        };

        _learningApiClient
            .Setup(x => x.Get<GetPagedShortCourseLearnersResponse>(It.IsAny<GetShortCourseLearningsForEarnings>()))
            .ReturnsAsync(new GetPagedShortCourseLearnersResponse { Items = [learning], TotalItems = 1 });

        _earningsApiClient
            .Setup(x => x.GetWithResponseCode<GetShortCourseDataResponse>(It.IsAny<GetShortCourseDataRequest>()))
            .ReturnsAsync(new ApiResponse<GetShortCourseDataResponse>(
                new GetShortCourseDataResponse { Earnings = [] },
                System.Net.HttpStatusCode.OK, ""));

        var query = new GetShortCourseEarningsQuery(12345678, 2526, 1, page: 1, pageSize: 20);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Learners.Should().HaveCount(1);
        result.Learners[0].LearnerRef.Should().BeEmpty();
    }
}
