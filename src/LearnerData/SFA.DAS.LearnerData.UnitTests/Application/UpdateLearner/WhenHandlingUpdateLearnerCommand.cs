using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
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
    private FeatureFlags _featureFlags;
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
        _featureFlags = new FeatureFlags { ApprenticeshipUpdateLearner = true };
        _sut = new UpdateLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _updateLearningPutRequestBuilder.Object,
            _updateEarningsOnProgrammeRequestBuilder.Object,
            _updateEarningsEnglishAndMathsRequestBuilder.Object,
            _updateEarningsLearningSupportRequestBuilder.Object,
            _distributedCache.Object,
            _featureFlags);
    }

    [Test]
    public async Task Then_Does_Not_Run_Update_Logic_If_Feature_Flag_Is_Disabled()
    {
        // Arrange
        _featureFlags.ApprenticeshipUpdateLearner = false;
        var command = _fixture.Create<UpdateLearnerCommand>();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _distributedCache.Verify(x => x.StoreLearner(It.IsAny<SFA.DAS.LearnerData.Requests.UpdateLearnerRequest>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        _updateLearningPutRequestBuilder.Verify(x => x.Build(It.IsAny<long>(), It.IsAny<SFA.DAS.LearnerData.Requests.UpdateLearnerRequest>(), It.IsAny<Guid>()), Times.Never);
        _learningApiClient.Verify(x => x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()), Times.Never);
        _earningsApiClient.VerifyNoOtherCalls();
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