using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Application.RemoveShortCourse;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Application.Requests.Learning;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

public class WhenHandlingRemoveShortCourseCommand
{
    private Fixture _fixture = new();

#pragma warning disable CS8618
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ILogger<RemoveShortCourseCommandHandler>> _logger;
    private Mock<IMessageSession> _messageSession;
    private RemoveShortCourseCommandHandler _sut;
    private PaymentsConfiguration _configuration;
    private string _learnerRef;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _logger = new Mock<ILogger<RemoveShortCourseCommandHandler>>();
        _configuration = new PaymentsConfiguration { PaymentsEndpoint = "destination" };

        _messageSession = new Mock<IMessageSession>();

        _sut = new RemoveShortCourseCommandHandler(
            _logger.Object, _learningApiClient.Object, _earningsApiClient.Object, _messageSession.Object, _configuration);

        _learnerRef = "someLearnerRef";
    }

    [Test]
    public async Task Then_Earnings_Called_Once_Per_Removed_Learning_When_Learning_Returns_200()
    {
        var command = _fixture.Create<RemoveShortCourseCommand>();
        var item1 = _fixture.Build<DeleteShortCourseItemResponse>()
            .With(x => x.Episodes, [new LearningInnerShortCourseEpisode
            {
                Ukprn = command.Ukprn,
                LearnerRef = _learnerRef,
                StartDate = DateTime.UtcNow.Date
            }])
            .Create();
        var item2 = _fixture.Build<DeleteShortCourseItemResponse>()
            .With(x => x.Episodes, [new LearningInnerShortCourseEpisode
            {
                Ukprn = command.Ukprn,
                LearnerRef = _learnerRef,
                StartDate = DateTime.UtcNow.Date
            }])
            .Create();
        var learningResponse = new DeleteShortCourseResponse { Results = [item1, item2] };

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.Is<DeleteShortCourseApiDeleteRequest>(r => r.Ukprn == command.Ukprn && r.LearnerKey == command.LearnerKey), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(learningResponse, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(_fixture.Create<DeleteShortCourseEarningsResponse>(), HttpStatusCode.NoContent, ""));

        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
            It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == item1.LearningKey && r.EpisodeKey == item1.RemovedEpisodeKey), true), Times.Once);
        _earningsApiClient.Verify(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
            It.Is<DeleteShortCourseEarningsRequest>(r => r.LearningKey == item2.LearningKey && r.EpisodeKey == item2.RemovedEpisodeKey), true), Times.Once);
    }

    [Test]
    public void Then_Throws_If_Learning_Delete_Fails()
    {
        var command = _fixture.Create<RemoveShortCourseCommand>();

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(_fixture.Create<DeleteShortCourseResponse>(), HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Then_Throws_If_Earnings_Delete_Fails()
    {
        var command = _fixture.Create<RemoveShortCourseCommand>();
        var item = _fixture.Build<DeleteShortCourseItemResponse>()
            .With(x => x.Episodes, [new LearningInnerShortCourseEpisode
            {
                Ukprn = command.Ukprn,
                LearnerRef = _learnerRef,
                StartDate = DateTime.UtcNow.Date
            }])
            .Create();
        var learningResponse = new DeleteShortCourseResponse { Results = [item] };

        _learningApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseResponse>(
                It.IsAny<DeleteShortCourseApiDeleteRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseResponse>(learningResponse, HttpStatusCode.OK, ""));

        _earningsApiClient.Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), true))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(_fixture.Create<DeleteShortCourseEarningsResponse>(), HttpStatusCode.InternalServerError, ""));

        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
