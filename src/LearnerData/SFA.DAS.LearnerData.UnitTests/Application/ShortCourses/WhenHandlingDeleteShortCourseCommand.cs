using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.DeleteShortCourse;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

public class WhenHandlingDeleteShortCourseCommand
{
    private Fixture _fixture;

#pragma warning disable CS8618
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ILogger<DeleteShortCourseCommandHandler>> _logger;
    private DeleteShortCourseCommandHandler _sut;
#pragma warning restore CS8618

    public WhenHandlingDeleteShortCourseCommand()
    {
        _fixture = new Fixture();
    }

    [SetUp]
    public void Setup()
    {
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _logger = new Mock<ILogger<DeleteShortCourseCommandHandler>>();
        _sut = new DeleteShortCourseCommandHandler(_logger.Object, _learningApiClient.Object, _earningsApiClient.Object);
    }

    [Test]
    public async Task Then_Earnings_Not_Called_When_Learning_Returns_200()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.Is<DeleteShortCourseApiDeleteRequest>(r => r.Ukprn == command.Ukprn && r.LearningKey == command.LearningKey), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, ""));

        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<NullResponse>(It.IsAny<DeleteShortCourseEarningsRequest>(), false), Times.Never);
    }

    [Test]
    public async Task Then_Earnings_Called_When_Learning_Returns_204()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.Is<DeleteShortCourseApiDeleteRequest>(r => r.Ukprn == command.Ukprn && r.LearningKey == command.LearningKey), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == command.LearningKey), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, ""));

        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<NullResponse>(
            It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == command.LearningKey), false), Times.Once);
    }

    [Test]
    public void Then_Throws_If_Learning_Delete_Fails()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Delete_Fails()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<NullResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
