using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.DeleteShortCourse;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using SFA.DAS.LearnerData.Configuration;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

public class WhenHandlingDeleteShortCourseCommand
{
    private Fixture _fixture;

#pragma warning disable CS8618
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ILogger<DeleteShortCourseCommandHandler>> _logger;
    private Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder> _calculateGrowthAndSkillsPaymentsEventBuilder;
    private Mock<IMessageSession> _messageSession;
    private DeleteShortCourseCommandHandler _sut;
    private PaymentsConfiguration _configuration;
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
        _configuration = new PaymentsConfiguration { PaymentsEndpoint = "destination" };

        _calculateGrowthAndSkillsPaymentsEventBuilder = new Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder>();
        _messageSession = new Mock<IMessageSession>();

        _calculateGrowthAndSkillsPaymentsEventBuilder.Setup(x => x.Build(It.IsAny<long>(), It.IsAny<IShortCourseLearningPaymentEventBuildContext>(), It.IsAny<ShortCourseEarningsResponse>()))
            .ReturnsAsync(_fixture.Create<CalculateGrowthAndSkillsPayments>());

        _sut = new DeleteShortCourseCommandHandler(
            _logger.Object, _learningApiClient.Object, _earningsApiClient.Object, _calculateGrowthAndSkillsPaymentsEventBuilder.Object, _messageSession.Object, _configuration);
    }

    [Test]
    public async Task Then_Earnings_Not_Called_When_Learning_Returns_204()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.Is<DeleteShortCourseApiDeleteRequest>(r => r.Ukprn == command.Ukprn && r.LearningKey == command.LearningKey), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(_fixture.Create<DeleteShortCourseResponse>(), HttpStatusCode.NoContent, ""));

        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(It.IsAny<DeleteShortCourseEarningsRequest>(), true), Times.Never);
    }

    [Test]
    public async Task Then_Earnings_Called_When_Learning_Returns_200()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.Is<DeleteShortCourseApiDeleteRequest>(r => r.Ukprn == command.Ukprn && r.LearningKey == command.LearningKey), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(_fixture.Create<DeleteShortCourseResponse>(), HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == command.LearningKey), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(_fixture.Create<DeleteShortCourseEarningsResponse>(), HttpStatusCode.NoContent, ""));

        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
            It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == command.LearningKey), true), Times.Once);
    }

    [Test]
    public void Then_Throws_If_Learning_Delete_Fails()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(_fixture.Create<DeleteShortCourseResponse>(), HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Delete_Fails()
    {
        var command = _fixture.Create<DeleteShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(_fixture.Create<DeleteShortCourseResponse>(), HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(_fixture.Create<DeleteShortCourseEarningsResponse>(), HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
