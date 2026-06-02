using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.CreateLearner;

public class WhenCreatingLearners
{
    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<IMessageSession> _mockMessageSession;
    private Mock<ILogger<CreateLearnerCommandHandler>> _mockLogger;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _mockLearningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _mockEarningsApiClient;
    private Mock<ICreateDraftLearningApiPostRequestBuilder> _mockCreateDraftLearningApiPostRequestBuilder;
    private Mock<IUpdateEarningsOnProgrammeRequestBuilder> _mockUpdateEarningsOnProgrammeRequestBuilder;
    private CreateLearnerCommandHandler _sut;


    public WhenCreatingLearners()
    {
        _fixture = new Fixture();
    }
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _mockMessageSession = new Mock<IMessageSession>();
        _mockLogger = new Mock<ILogger<CreateLearnerCommandHandler>>();
        _mockLearningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _mockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _mockCreateDraftLearningApiPostRequestBuilder = new Mock<ICreateDraftLearningApiPostRequestBuilder>();
        _mockUpdateEarningsOnProgrammeRequestBuilder = new Mock<IUpdateEarningsOnProgrammeRequestBuilder>();

        _sut = new CreateLearnerCommandHandler(
            _mockLogger.Object,
            _mockMessageSession.Object,
            _mockLearningApiClient.Object,
            _mockCreateDraftLearningApiPostRequestBuilder.Object,
            _mockEarningsApiClient.Object,
            _mockUpdateEarningsOnProgrammeRequestBuilder.Object);

        _mockCreateDraftLearningApiPostRequestBuilder
            .Setup(x => x.Build(It.IsAny<long>(), It.IsAny<CreateLearnerRequest>()))
            .Returns(new CreateDraftLearningApiPostRequest(new UpdateLearningRequestBody(), 0));

        var successResponse = new ApiResponse<CreateDraftLearnerApiPutResponse>(
            new CreateDraftLearnerApiPutResponse { Changes = new List<BaseLearnerApiPutResponse.LearningUpdateChanges>() },
            HttpStatusCode.OK,
            string.Empty);

        _mockLearningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(It.IsAny<CreateDraftLearningApiPostRequest>(), true))
            .ReturnsAsync(successResponse);
    }


    [Test]
    public async Task Then_call_is_successful()
    {
        // Arrange
        var command = GetProcessLearnersCommand();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockMessageSession.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()),
            Times.Once());
    }

    [Test]
    public async Task Then_call_is_successful_and_request_fields_maps_to_event()
    {
        // Arrange
        var request = _fixture.Create<CreateLearnerRequest>();
        var command = GetProcessLearnersCommand(request);

        var @event = new LearnerDataEvent();
        _mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        @event.Should().BeEquivalentTo(new
        {
            ULN = request.Learner.Uln,
            UKPRN = command.Ukprn,
            FirstName = request.Learner.FirstName,
            LastName = request.Learner.LastName,
            Email = request.Learner.Email,
            DoB = request.Learner.Dob!.Value,
            StartDate = request.Delivery.OnProgramme.First().StartDate,
            PlannedEndDate = request.Delivery.OnProgramme.First().ExpectedEndDate,
            PercentageLearningToBeDelivered = request.Delivery.OnProgramme.First().PercentageOfTrainingLeft,
            EpaoPrice = request.Delivery.OnProgramme.First().Costs.First().EpaoPrice,
            TrainingPrice = request.Delivery.OnProgramme.First().Costs.First().TrainingPrice,
            AgreementId = request.Delivery.OnProgramme.First().AgreementId,
            IsFlexiJob = request.Delivery.OnProgramme.First().IsFlexiJob!.Value,
            StandardCode = request.Delivery.OnProgramme.First().StandardCode,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = request.ConsumerReference
        });
    }

    [Test]
    public void Then_throws_exception_if_learner_creation_fails()
    {
        // Arrange
        var command = GetProcessLearnersCommand();
        var failureResponse = new ApiResponse<CreateDraftLearnerApiPutResponse>(
            new CreateDraftLearnerApiPutResponse(),
            HttpStatusCode.InternalServerError,
            "Internal Error");

        _mockLearningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(It.IsAny<CreateDraftLearningApiPostRequest>(), true))
            .ReturnsAsync(failureResponse);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Then_reinstates_earnings_if_learner_is_reinstated()
    {
        // Arrange
        var command = GetProcessLearnersCommand();
        var learningKey = Guid.NewGuid();
        var responseBody = new CreateDraftLearnerApiPutResponse
        {
            LearningKey = learningKey,
            Changes = new List<BaseLearnerApiPutResponse.LearningUpdateChanges> { BaseLearnerApiPutResponse.LearningUpdateChanges.Reinstated }
        };
        var successResponse = new ApiResponse<CreateDraftLearnerApiPutResponse>(
            responseBody,
            HttpStatusCode.OK,
            string.Empty);

        _mockLearningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(It.IsAny<CreateDraftLearningApiPostRequest>(), true))
            .ReturnsAsync(successResponse);

        var earningsPutRequest = _fixture.Create<UpdateOnProgrammeApiPutRequest>();
        _mockUpdateEarningsOnProgrammeRequestBuilder
            .Setup(x => x.Build(learningKey, command.Request, responseBody, It.IsAny<UpdateLearningRequestBody>()))
            .ReturnsAsync(earningsPutRequest);

        _mockEarningsApiClient
            .Setup(x => x.Put(It.IsAny<UpdateOnProgrammeApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUpdateEarningsOnProgrammeRequestBuilder.Verify(x => x.Build(learningKey, command.Request, responseBody, It.IsAny<UpdateLearningRequestBody>()), Times.Once);
        _mockEarningsApiClient.Verify(x => x.Put(earningsPutRequest), Times.Once);
    }

    [Test]
    public async Task Then_does_not_reinstate_earnings_if_learner_is_not_reinstated()
    {
        // Arrange
        var command = GetProcessLearnersCommand();
        var responseBody = new CreateDraftLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            Changes = new List<BaseLearnerApiPutResponse.LearningUpdateChanges> { BaseLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails }
        };
        var successResponse = new ApiResponse<CreateDraftLearnerApiPutResponse>(
            responseBody,
            HttpStatusCode.OK,
            string.Empty);

        _mockLearningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(It.IsAny<CreateDraftLearningApiPostRequest>(), true))
            .ReturnsAsync(successResponse);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUpdateEarningsOnProgrammeRequestBuilder.Verify(x => x.Build(It.IsAny<Guid>(), It.IsAny<CreateLearnerRequest>(), It.IsAny<BaseLearnerApiPutResponse>(), It.IsAny<UpdateLearningRequestBody>()), Times.Never);
        _mockEarningsApiClient.Verify(x => x.Put(It.IsAny<UpdateOnProgrammeApiPutRequest>()), Times.Never);
    }

    [Test]
    public async Task Then_does_not_reinstate_earnings_if_response_body_is_null()
    {
        // Arrange
        var command = GetProcessLearnersCommand();
        var successResponse = new ApiResponse<CreateDraftLearnerApiPutResponse>(
            null!,
            System.Net.HttpStatusCode.OK,
            string.Empty);

        _mockLearningApiClient
            .Setup(x => x.PostWithResponseCode<CreateDraftLearnerApiPutResponse>(It.IsAny<CreateDraftLearningApiPostRequest>(), true))
            .ReturnsAsync(successResponse);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockUpdateEarningsOnProgrammeRequestBuilder.Verify(x => x.Build(It.IsAny<Guid>(), It.IsAny<CreateLearnerRequest>(), It.IsAny<BaseLearnerApiPutResponse>(), It.IsAny<UpdateLearningRequestBody>()), Times.Never);
        _mockEarningsApiClient.Verify(x => x.Put(It.IsAny<UpdateOnProgrammeApiPutRequest>()), Times.Never);
    }

    private CreateLearnerCommand GetProcessLearnersCommand(CreateLearnerRequest? createLearnerRequest = null)
    {
        var command = _fixture.Create<CreateLearnerCommand>();
        if (createLearnerRequest != null)
        {
            command.Request = createLearnerRequest;
        }

        command.Request.Learner.Uln = _fixture.Create<long>();
        command.Request.Delivery.OnProgramme.First().Costs = new List<CostDetails> { _fixture.Create<CostDetails>() };

        return command;
    }
}