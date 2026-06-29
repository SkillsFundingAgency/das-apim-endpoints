using System.Net;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateShortCourseLearning;
using SFA.DAS.LearnerData.Application.Requests.Earnings;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using OnProgramme = SFA.DAS.LearnerData.Requests.LearningInner.OnProgramme;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Services.ShortCourses;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingCreateDraftShortCourseCommand
{
    private CreateDraftShortCourseCommandHandler _handler;
    private Mock<ILogger<CreateDraftShortCourseCommandHandler>> _logger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ICreateDraftShortCoursePostRequestBuilder> _createDraftShortCoursePostRequestBuilder;
    private Mock<ICreateUnapprovedShortCourseLearningRequestBuilder> _createUnapprovedShortCourseLearningRequestBuilder;
    private Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder> _updateShortCourseOnProgrammeEarningPutRequestBuilder;
    private Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder> _calculateGrowthAndSkillsPaymentsEventBuilder;
    private Mock<IMessageSession> _messageSession;
    private PaymentsConfiguration _paymentsConfiguration;

    private CreateDraftShortCourseCommand _command;
    private CreateDraftShortCourseRequest _builtRequest;
    private OnProgramme _resolvedOnProg;
    private long _ukprn;
    private Guid _learningKey;
    private Guid _episodeKey;
    private ShortCourseRequest _shortCourseRequest;
    private ShortCourseOnProgramme _onProg;
    private CreateUnapprovedShortCourseLearningRequest _builtEarningsRequest;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateDraftShortCourseCommandHandler>>();
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _createDraftShortCoursePostRequestBuilder = new Mock<ICreateDraftShortCoursePostRequestBuilder>();
        _createUnapprovedShortCourseLearningRequestBuilder = new Mock<ICreateUnapprovedShortCourseLearningRequestBuilder>();
        _updateShortCourseOnProgrammeEarningPutRequestBuilder = new Mock<IUpdateShortCourseOnProgrammeEarningPutRequestBuilder>();
        _calculateGrowthAndSkillsPaymentsEventBuilder = new Mock<ICalculateGrowthAndSkillsPaymentsEventBuilder>();
        _messageSession = new Mock<IMessageSession>();
        _paymentsConfiguration = new PaymentsConfiguration { PaymentsEndpoint = "payments-endpoint" };

        _calculateGrowthAndSkillsPaymentsEventBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<IShortCourseLearningPaymentEventBuildContext>(), It.IsAny<ShortCourseEarningsResponse>()))
            .ReturnsAsync(new CalculateGrowthAndSkillsPayments());

        _handler = new CreateDraftShortCourseCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _createDraftShortCoursePostRequestBuilder.Object,
            _createUnapprovedShortCourseLearningRequestBuilder.Object,
            _updateShortCourseOnProgrammeEarningPutRequestBuilder.Object,
            _calculateGrowthAndSkillsPaymentsEventBuilder.Object,
            _messageSession.Object,
            _paymentsConfiguration);

        // Arrange
        _ukprn = 12345;
        _learningKey = Guid.NewGuid();
        _episodeKey = Guid.NewGuid();

        _builtEarningsRequest = new CreateUnapprovedShortCourseLearningRequest();

        _onProg = new ShortCourseOnProgramme { StartDate = new DateTime(2025, 8, 1), AgreementId = "AGR-001" };

        _shortCourseRequest = new ShortCourseRequest
        {
            Delivery = new ShortCourseDelivery
            {
                OnProgramme = [_onProg]
            },
            ConsumerReference = "consumer-ref-123"
        };

        _command = new CreateDraftShortCourseCommand
        {
            Ukprn = _ukprn,
            AcademicYear = 2526,
            ShortCourseRequest = _shortCourseRequest
        };

        _resolvedOnProg = new OnProgramme
        {
            StartDate = new DateTime(2025, 8, 1),
            ExpectedEndDate = new DateTime(2026, 7, 31),
            Price = 1500,
            EmployerId = 123456,
            CourseCode = "91",
            WithdrawalReasonCode = 2
        };

        _builtRequest = new CreateDraftShortCourseRequest
        {
            LearnerUpdateDetails = new()
            {
                Uln = 99999999,
                FirstName = "John",
                LastName = "Smith",
                EmailAddress = "john@test.com",
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            OnProgramme = [_resolvedOnProg]
        };

        _createDraftShortCoursePostRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, _ukprn, It.IsAny<int>()))
            .ReturnsAsync(_builtRequest);

        var apiResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(
            new CreateDraftShortCoursePostResponse { Results = [new CreateShortCoursePostResponse { LearningKey = _learningKey, EpisodeKey = _episodeKey }] },
            HttpStatusCode.Created, "");

        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(apiResponse);

        _createUnapprovedShortCourseLearningRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, _onProg, _learningKey, _episodeKey, _ukprn, _resolvedOnProg))
            .Returns(_builtEarningsRequest);
    }

    [Test]
    public async Task Then_Learning_Is_Updated_With_ShortCourse()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
                x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(
                    It.Is<CreateDraftShortCourseApiPostRequest>(r => r.Data == _builtRequest)),
            Times.Once);
    }

    [Test]
    public async Task Then_A_LearnerDataEvent_Is_Published()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x =>
            x.Publish(
                It.Is<LearnerDataEvent>(e =>
                    e.ULN == _builtRequest.LearnerUpdateDetails.Uln &&
                    e.UKPRN == _ukprn &&
                    e.FirstName == _builtRequest.LearnerUpdateDetails.FirstName &&
                    e.LastName == _builtRequest.LearnerUpdateDetails.LastName &&
                    e.Email == _builtRequest.LearnerUpdateDetails.EmailAddress &&
                    e.DoB == _builtRequest.LearnerUpdateDetails.DateOfBirth &&
                    e.StartDate == _resolvedOnProg.StartDate &&
                    e.PlannedEndDate == _resolvedOnProg.ExpectedEndDate &&
                    e.PercentageLearningToBeDelivered == 100 &&
                    e.EpaoPrice == 0 &&
                    e.TrainingPrice == (int)_resolvedOnProg.Price &&
                    e.IsFlexiJob == false &&
                    e.PlannedOTJTrainingHours == 0 &&
                    e.AgreementId == _onProg.AgreementId &&
                    e.ConsumerReference == _shortCourseRequest.ConsumerReference &&
                    e.StandardCode == 0 &&
                    e.LarsCode == _resolvedOnProg.CourseCode &&
                    e.CorrelationId != Guid.Empty &&
                    e.LearningType == (LearningType)_resolvedOnProg.LearningType
                ),
                It.IsAny<PublishOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_The_Result_CorrelationId_Matches_The_Published_Event()
    {
        // Arrange
        LearnerDataEvent publishedEvent = null;
        _messageSession
            .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()))
            .Callback<object, PublishOptions>((e, _) => publishedEvent = e as LearnerDataEvent)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.CorrelationId.Should().Be(publishedEvent!.CorrelationId);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_Unapproved_ShortCourse_Learning()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()));
    }

    [Test]
    public async Task Then_When_Learning_Returns_NoContent_Earnings_Is_Not_Called()
    {
        // Arrange
        var noContentResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(null, HttpStatusCode.NoContent, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(noContentResponse);

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()), Times.Never);
    }

    [Test]
    public async Task Then_When_Learning_Returns_NoContent_A_Result_Is_Returned()
    {
        // Arrange
        var noContentResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(null, HttpStatusCode.NoContent, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(noContentResponse);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Then_When_Reinstated_Earnings_Is_Updated_Via_Put()
    {
        // Arrange
        SetupReinstatedLearningResponse();

        var builtBody = new UpdateShortCourseOnProgrammeRequestBody { Milestones = [] };
        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(_resolvedOnProg))
            .Returns(builtBody);

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(new UpdateShortCourseEarningPutResponse(), HttpStatusCode.OK, ""));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.Is<UpdateShortCourseOnProgrammeEarningPutRequest>(r =>
                    r.Data == builtBody && r.PutUrl.Contains(_learningKey.ToString()))),
            Times.Once);
        _earningsApiClient.Verify(x => x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()), Times.Never);
        _messageSession.Verify(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()), Times.Never);
    }

    [Test]
    public async Task Then_When_Reinstated_CalculateGrowthAndSkillsPayments_Is_Sent_And_Event_Published()
    {
        // Arrange
        SetupReinstatedLearningResponse();

        _updateShortCourseOnProgrammeEarningPutRequestBuilder
            .Setup(x => x.Build(_resolvedOnProg))
            .Returns(new UpdateShortCourseOnProgrammeRequestBody { Milestones = [] });

        _earningsApiClient
            .Setup(x => x.PutWithResponseCode<UpdateShortCourseOnProgrammeRequestBody, UpdateShortCourseEarningPutResponse>(
                It.IsAny<UpdateShortCourseOnProgrammeEarningPutRequest>()))
            .ReturnsAsync(new ApiResponse<UpdateShortCourseEarningPutResponse>(new UpdateShortCourseEarningPutResponse(), HttpStatusCode.OK, ""));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x => x.Send(It.IsAny<CalculateGrowthAndSkillsPayments>(), It.IsAny<SendOptions>()), Times.Once);
        _messageSession.Verify(x => x.Publish(It.IsAny<GrowthAndSkillsPaymentsRecalculatedEvent>(), It.IsAny<PublishOptions>()), Times.Once);
    }

    [Test]
    public async Task Then_When_Not_Reinstated_A_LearnerDataEvent_Is_Published()
    {
        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _messageSession.Verify(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()), Times.Once);
    }

    [Test]
    public async Task Then_Bundled_Post_Processes_Each_Item_Independently()
    {
        // Arrange - AC3/AC4 shape: original (ignored, e.g. flag-off Progression) course alongside a new course in one POST.
        var secondOnProg = new ShortCourseOnProgramme { StartDate = new DateTime(2025, 9, 1), AgreementId = "AGR-002" };
        _shortCourseRequest.Delivery.OnProgramme = [_onProg, secondOnProg];

        var secondResolvedOnProg = new OnProgramme { StartDate = new DateTime(2025, 9, 1), CourseCode = "92" };
        _builtRequest.OnProgramme = [_resolvedOnProg, secondResolvedOnProg];

        var secondLearningKey = Guid.NewGuid();
        var secondEpisodeKey = Guid.NewGuid();
        var apiResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(
            new CreateDraftShortCoursePostResponse
            {
                Results =
                [
                    new CreateShortCoursePostResponse { IsIgnored = true },
                    new CreateShortCoursePostResponse { LearningKey = secondLearningKey, EpisodeKey = secondEpisodeKey }
                ]
            },
            HttpStatusCode.Created, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(apiResponse);

        _createUnapprovedShortCourseLearningRequestBuilder
            .Setup(x => x.Build(_shortCourseRequest, secondOnProg, secondLearningKey, secondEpisodeKey, _ukprn, secondResolvedOnProg))
            .Returns(new CreateUnapprovedShortCourseLearningRequest());

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Post(It.IsAny<PostCreateUnapprovedShortCourseLearningRequest>()), Times.Once);
        _messageSession.Verify(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()), Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Delete_Is_Called_For_Omitted_Learning()
    {
        // Arrange - the Learning inner appends IsRemoved results after the matched OnProgramme items,
        // so the response list can be longer than the request's OnProgramme array.
        var removedLearningKey = Guid.NewGuid();
        var removedEpisodeKey = Guid.NewGuid();

        var apiResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(
            new CreateDraftShortCoursePostResponse
            {
                Results =
                [
                    new CreateShortCoursePostResponse { LearningKey = _learningKey, EpisodeKey = _episodeKey },
                    new CreateShortCoursePostResponse { IsRemoved = true, LearningKey = removedLearningKey, EpisodeKey = removedEpisodeKey, CourseCode = "TEST02" }
                ]
            },
            HttpStatusCode.Created, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(apiResponse);

        _earningsApiClient
            .Setup(x => x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.IsAny<DeleteShortCourseEarningsRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<DeleteShortCourseEarningsResponse>(
                new DeleteShortCourseEarningsResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await _handler.Handle(_command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x =>
            x.DeleteWithResponseCode<DeleteShortCourseEarningsResponse>(
                It.Is<DeleteShortCourseEarningsRequest>(r =>
                    r.LearningKey == removedLearningKey && r.EpisodeKey == removedEpisodeKey),
                It.IsAny<bool>()),
            Times.Once);
    }

    private void SetupReinstatedLearningResponse()
    {
        var reinstatedResponse = new ApiResponse<CreateDraftShortCoursePostResponse>(
            new CreateDraftShortCoursePostResponse { Results = [new CreateShortCoursePostResponse { LearningKey = _learningKey, EpisodeKey = _episodeKey, IsReinstated = true, Episodes = [] }] },
            HttpStatusCode.OK, "");
        _learningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftShortCoursePostResponse>(It.IsAny<CreateDraftShortCourseApiPostRequest>()))
            .ReturnsAsync(reinstatedResponse);
    }
}
