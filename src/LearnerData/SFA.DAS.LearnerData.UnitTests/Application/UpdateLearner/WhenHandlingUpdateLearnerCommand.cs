using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using SFA.DAS.LearnerData.Services;
using Microsoft.Extensions.Caching.Distributed;

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
    private Mock<IDistributedCache> _distributedCache;
    private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
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
        _distributedCache = new Mock<IDistributedCache>();
        _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
        _sut = new UpdateLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _updateLearningPutRequestBuilder.Object,
            _updateEarningsOnProgrammeRequestBuilder.Object,
            _updateEarningsEnglishAndMathsRequestBuilder.Object,
            _updateEarningsLearningSupportRequestBuilder.Object,
            _distributedCache.Object);
    }

    [Test]
    public async Task Then_Learning_Is_Updated()
    {
        //Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var apiPutRequest = MockLearningPutRequestBuilder(command);
        MockLearningApiResponse();

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
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        var updateOnProgPutRequest = _fixture.Create<UpdateOnProgrammeApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate); // on-prog change

        _updateEarningsOnProgrammeRequestBuilder.Setup(x => x.Build(command, updateLearningApiResponse, apiPutRequest))
            .ReturnsAsync(updateOnProgPutRequest);

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);

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
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        var updateLearningSupportApiPutRequest = _fixture.Create<UpdateLearningSupportApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport); // LSF change

        _updateEarningsLearningSupportRequestBuilder.Setup(x => x.Build(command, updateLearningApiResponse, apiPutRequest))
            .Returns(updateLearningSupportApiPutRequest);

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);

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
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        var englishAndMathsApiPutRequest = _fixture.Create<UpdateEnglishAndMathsApiPutRequest>();

        var updateLearningApiResponse = _fixture.Create<UpdateLearnerApiPutResponse>();
        updateLearningApiResponse.Changes.Clear();
        updateLearningApiResponse.Changes.Add(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish); // E&M change

        _updateEarningsEnglishAndMathsRequestBuilder.Setup(x => x.Build(command, updateLearningApiResponse, apiPutRequest))
            .Returns(englishAndMathsApiPutRequest);

        MockLearningApiResponse(_learningApiClient, updateLearningApiResponse, HttpStatusCode.OK);

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
    protected void MockLearningApiResponse()
    {
        var responseBody = new UpdateLearnerApiPutResponse();
        var response = new ApiResponse<UpdateLearnerApiPutResponse>(responseBody, HttpStatusCode.OK, string.Empty);
        _learningApiClient.Setup(x =>
                x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()))
            .ReturnsAsync(response);
    }

    protected UpdateLearningApiPutRequest MockLearningPutRequestBuilder(UpdateLearnerCommand command)
    {
        var fixture = new Fixture();
        var apiPutRequest = fixture.Create<UpdateLearningApiPutRequest>();
        _updateLearningPutRequestBuilder.Setup(x => x.Build(command)).Returns(apiPutRequest);
        return apiPutRequest;
    }

    protected static void MockLearningApiResponse(
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
    }
}