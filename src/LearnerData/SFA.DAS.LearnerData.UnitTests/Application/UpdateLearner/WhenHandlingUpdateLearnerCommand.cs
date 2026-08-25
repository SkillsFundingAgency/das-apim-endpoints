using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.Apim.Shared.Models;
using System.Net;
using SFA.DAS.SharedOuterApi.Types.Configuration;

namespace SFA.DAS.LearnerData.UnitTests.Application.UpdateLearner;

public class WhenHandlingUpdateLearnerCommand
{
    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<IUpdateLearningPutRequestBuilder> _updateLearningPutRequestBuilder;
    private Mock<IUpdateEarningsOnProgrammeRequestBuilder> _updateEarningsOnProgrammeRequestBuilder;
    private Mock<IUpdateEarningsLearningSupportRequestBuilder> _updateEarningsLearningSupportRequestBuilder;
    private Mock<IUpdateEarningsEnglishAndMathsRequestBuilder> _updateEarningsEnglishAndMathsRequestBuilder;
    private Mock<ILearnerDataCacheService> _distributedCache;
    private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
    private Mock<IMessageSession> _messageSession;
    private Mock<IApprovedApprenticeshipExistsChecker> _approvedApprenticeshipExistsChecker;
    private Mock<ICourseService> _courseService;
    private Mock<ILearnerDataEventMapper> _learnerDataEventMapper;
    private UpdateLearnerCommandHandler _sut;
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _updateLearningPutRequestBuilder = new Mock<IUpdateLearningPutRequestBuilder>();
        _updateEarningsOnProgrammeRequestBuilder = new Mock<IUpdateEarningsOnProgrammeRequestBuilder>();
        _updateEarningsEnglishAndMathsRequestBuilder = new Mock<IUpdateEarningsEnglishAndMathsRequestBuilder>();
        _updateEarningsLearningSupportRequestBuilder = new Mock<IUpdateEarningsLearningSupportRequestBuilder>();
        _distributedCache = new Mock<ILearnerDataCacheService>();
        _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
        _messageSession = new Mock<IMessageSession>();
        _approvedApprenticeshipExistsChecker = new Mock<IApprovedApprenticeshipExistsChecker>();
        _courseService = new Mock<ICourseService>();
        _learnerDataEventMapper = new Mock<ILearnerDataEventMapper>();
        _sut = new UpdateLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _updateLearningPutRequestBuilder.Object,
            _updateEarningsOnProgrammeRequestBuilder.Object,
            _updateEarningsEnglishAndMathsRequestBuilder.Object,
            _updateEarningsLearningSupportRequestBuilder.Object,
            _distributedCache.Object,
            _messageSession.Object,
            _approvedApprenticeshipExistsChecker.Object,
            _courseService.Object,
            _learnerDataEventMapper.Object);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);
    }

    [Test]
    public async Task Then_Learning_Is_Updated()
    {
        //Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse();
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        //Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(apiPutRequest));
    }

    [Test]
    public async Task Then_Earnings_Is_Not_Updated_If_No_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Throws_Error_If_Learner_Update_Fails()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.InternalServerError, "error");

        // Act/Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Handle(command, CancellationToken.None));
    }


    [Test]
    public async Task Then_Earnings_Is_Updated_With_OnProgrammeUpdates()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        var updateOnProgPutRequest = _fixture.Create<UpdateOnProgrammeApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate); // on-prog change

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        _updateEarningsOnProgrammeRequestBuilder.Setup(x => x.Build(command.UpdateLearnerRequest, updateLearningApiResponse, apiPutRequest.Data))
            .ReturnsAsync(updateOnProgPutRequest);

        _earningsApiClient.Setup(x => x.Put(It.IsAny<UpdateOnProgrammeApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x => x.Put(
                It.Is<UpdateOnProgrammeApiPutRequest>(r => r == updateOnProgPutRequest)),
            Times.Once);

        _earningsApiClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_LearningSupport_Updates()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        var updateLearningSupportApiPutRequest = _fixture.Create<UpdateLearningSupportApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport); // LSF change

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        _updateEarningsLearningSupportRequestBuilder.Setup(x => x.Build(updateLearningApiResponse, apiPutRequest))
            .Returns(updateLearningSupportApiPutRequest);

        _earningsApiClient.Setup(x => x.Put(It.IsAny<UpdateLearningSupportApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x => x.Put(
                It.Is<UpdateLearningSupportApiPutRequest>(r => r == updateLearningSupportApiPutRequest)),
            Times.Once);

        _earningsApiClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_EnglishAndMaths_Updates()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        var englishAndMathsApiPutRequest = _fixture.Create<UpdateEnglishAndMathsApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.EnglishAndMaths); // E&M change

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        _updateEarningsEnglishAndMathsRequestBuilder.Setup(x => x.Build(command, updateLearningApiResponse, apiPutRequest))
            .Returns(englishAndMathsApiPutRequest);

        _earningsApiClient.Setup(x => x.Put(It.IsAny<UpdateEnglishAndMathsApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x => x.Put(
                It.Is<UpdateEnglishAndMathsApiPutRequest>(r => r == englishAndMathsApiPutRequest)),
            Times.Once);

        _earningsApiClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Publishes_A_LearnerDataEvent_When_No_Approved_Apprenticeship_Exists()
    {
        // Arrange
        var onProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2025, 9, 1));
        var command = BuildCommand(onProgramme);
        MockLearningApiResponse();
        MockLearningPutRequestBuilder(command);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln.ToString(), 1, new DateTime(2025, 9, 1)))
            .ReturnsAsync(false);

        _courseService.Setup(x => x.GetStandardDetailsById("1"))
            .ReturnsAsync(new StandardDetailResponse { ApprenticeshipType = "Apprenticeship" });

        var evt = _fixture.Create<LearnerDataEvent>();
        _learnerDataEventMapper
            .Setup(x => x.Build(command.Ukprn, command.UpdateLearnerRequest.Learner, onProgramme, LearningType.Apprenticeship, command.CorrelationId, command.ReceivedOn, command.UpdateLearnerRequest.ConsumerReference))
            .Returns(evt);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x => x.Publish(evt, It.IsAny<PublishOptions>()), Times.Once);
    }

    [Test]
    public async Task Then_Does_Not_Publish_A_LearnerDataEvent_When_An_Approved_Apprenticeship_Already_Exists()
    {
        // Arrange
        var onProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2025, 9, 1));
        var command = BuildCommand(onProgramme);
        MockLearningApiResponse();
        MockLearningPutRequestBuilder(command);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _messageSession.VerifyNoOtherCalls();
        _courseService.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Returns_From_BreakInLearning_Do_Not_Result_In_An_Extra_LearnerDataEvent()
    {
        // Arrange - two OnProgs with the same StandardCode and AgreementId - Apprenticeship plus BiL return
        var earlierOnProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2025, 9, 1));
        var laterOnProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2026, 1, 1));
        var command = BuildCommand(earlierOnProgramme, laterOnProgramme);
        MockLearningApiResponse();
        MockLearningPutRequestBuilder(command);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(false);

        _courseService.Setup(x => x.GetStandardDetailsById("1"))
            .ReturnsAsync(new StandardDetailResponse { ApprenticeshipType = "Apprenticeship" });

        var evt = _fixture.Create<LearnerDataEvent>();
        _learnerDataEventMapper
            .Setup(x => x.Build(command.Ukprn, command.UpdateLearnerRequest.Learner, earlierOnProgramme, LearningType.Apprenticeship, command.CorrelationId, command.ReceivedOn, command.UpdateLearnerRequest.ConsumerReference))
            .Returns(evt);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert - only one LearnerDataEvent published for the whole group, built from the earliest item (the original apprenticeship)
        _approvedApprenticeshipExistsChecker.Verify(x =>
            x.Exists(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln.ToString(), 1, new DateTime(2025, 9, 1)), Times.Once);
        _approvedApprenticeshipExistsChecker.VerifyNoOtherCalls();

        _messageSession.Verify(x => x.Publish(evt, It.IsAny<PublishOptions>()), Times.Once);
        _messageSession.Verify(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()), Times.Once);
    }

    [Test]
    public async Task Then_Returns_From_BreakInLearning_Do_Not_Result_In_A_LearnerDataEvent_When_Already_Approved()
    {
        // Arrange - two OnProgs with the same StandardCode and AgreementId - Apprenticeship plus BiL return,
        // where the original apprenticeship is already approved.
        var earlierOnProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2025, 9, 1));
        var laterOnProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2026, 1, 1));
        var command = BuildCommand(earlierOnProgramme, laterOnProgramme);
        MockLearningApiResponse();
        MockLearningPutRequestBuilder(command);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert - neither the original nor the BiL return item results in a LearnerDataEvent
        _approvedApprenticeshipExistsChecker.Verify(x =>
            x.Exists(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln.ToString(), 1, new DateTime(2025, 9, 1)), Times.Once);
        _approvedApprenticeshipExistsChecker.VerifyNoOtherCalls();

        _messageSession.VerifyNoOtherCalls();
        _courseService.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Then_Checks_Approval_Separately_For_Different_Standard_And_Agreement_Combinations()
    {
        // Arrange
        var approvedOnProgramme = BuildOnProgramme(standardCode: 1, agreementId: "A1", startDate: new DateTime(2025, 9, 1));
        var newOnProgramme = BuildOnProgramme(standardCode: 2, agreementId: "A2", startDate: new DateTime(2026, 1, 1));
        var command = BuildCommand(approvedOnProgramme, newOnProgramme);
        MockLearningApiResponse();
        MockLearningPutRequestBuilder(command);

        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln.ToString(), 1, new DateTime(2025, 9, 1)))
            .ReturnsAsync(true);
        _approvedApprenticeshipExistsChecker
            .Setup(x => x.Exists(command.Ukprn, command.UpdateLearnerRequest.Learner.Uln.ToString(), 2, new DateTime(2026, 1, 1)))
            .ReturnsAsync(false);

        _courseService.Setup(x => x.GetStandardDetailsById("2"))
            .ReturnsAsync(new StandardDetailResponse { ApprenticeshipType = "Apprenticeship" });

        var evt = _fixture.Create<LearnerDataEvent>();
        _learnerDataEventMapper
            .Setup(x => x.Build(command.Ukprn, command.UpdateLearnerRequest.Learner, newOnProgramme, LearningType.Apprenticeship, command.CorrelationId, command.ReceivedOn, command.UpdateLearnerRequest.ConsumerReference))
            .Returns(evt);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert - only the genuinely new (StandardCode, AgreementId) combination gets published
        _messageSession.Verify(x => x.Publish(evt, It.IsAny<PublishOptions>()), Times.Once);
        _courseService.Verify(x => x.GetStandardDetailsById("1"), Times.Never);
    }

    private UpdateLearnerCommand BuildCommand(params OnProgrammeRequestDetails[] onProgramme)
    {
        var command = _fixture.Create<UpdateLearnerCommand>();
        command.UpdateLearnerRequest.Delivery.OnProgramme = onProgramme.ToList();
        return command;
    }

    private static OnProgrammeRequestDetails BuildOnProgramme(int standardCode, string agreementId, DateTime startDate)
    {
        var fixture = new Fixture();
        var onProgramme = fixture.Create<OnProgrammeRequestDetails>();
        onProgramme.StandardCode = standardCode;
        onProgramme.AgreementId = agreementId;
        onProgramme.StartDate = startDate;
        return onProgramme;
    }

    /// <returns>LearningKey</returns>
    protected Guid MockLearningApiResponse()
    {
        var responseBody = new UpdateLearnerApiPutResponse();
        var response = new ApiResponse<UpdateLearnerApiPutResponse>(responseBody, HttpStatusCode.OK, string.Empty);
        _learningApiClient.Setup(x =>
                x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()))
            .ReturnsAsync(response);

        return responseBody.LearningKey;
    }

    /// <returns>LearningKey</returns>
    protected Guid MockLearningApiResponse(
        Mock<ILearningApiClient<LearningApiConfiguration>> learningApiClient,
        UpdateLearnerApiPutResponse responseBody,
        HttpStatusCode statusCode,
        string errorContent = "")
    {
        var response = new ApiResponse<UpdateLearnerApiPutResponse>(
            responseBody,
            statusCode,
            errorContent);

        learningApiClient.Setup(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()))
        .ReturnsAsync(response);

        return responseBody.LearningKey;
    }

    protected UpdateLearningApiPutRequest MockLearningPutRequestBuilder(UpdateLearnerCommand command)
    {
        var fixture = new Fixture();
        var apiPutRequest = fixture.Create<UpdateLearningApiPutRequest>();
        _updateLearningPutRequestBuilder.Setup(x => x.Build(command.Ukprn, command.UpdateLearnerRequest, command.LearnerKey)).Returns(apiPutRequest);
        return apiPutRequest;
    }
}