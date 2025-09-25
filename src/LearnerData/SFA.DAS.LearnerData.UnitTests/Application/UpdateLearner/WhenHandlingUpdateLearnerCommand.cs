using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.UpdateLearner;

public class WhenHandlingUpdateLearnerCommand
{
    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
    private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
    private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
    private UpdateLearnerCommandHandler _sut;


    public WhenHandlingUpdateLearnerCommand()
    {
        _fixture = new Fixture();    
    }
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
        _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
        _sut = new UpdateLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedCompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.CompletionDate;

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Learner.CompletionDate == expectedCompletionDate)), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveCompletionApiPatchRequest>(
            r => r.Data.CompletionDate == expectedCompletionDate)), Times.Once);
    }

    [Test]
    public async Task Then_No_Earnings_Updated_If_No_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()), Times.Never);
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
    public async Task Then_Learner_Is_Updated_Successfully_With_MathsAndEnglish_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedCompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.CompletionDate;
        var expectedMathsAndEnglishCourses = command.UpdateLearnerRequest.Delivery.EnglishAndMaths;

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Learner.CompletionDate == expectedCompletionDate)), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveMathsAndEnglishApiPatchRequest>(
            r => Matches(r.Data, expectedMathsAndEnglishCourses))), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_LearningSupport_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedLearningSupport = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.SelectMany(x=>x.LearningSupport).ToList();
        expectedLearningSupport.AddRange(command.UpdateLearnerRequest.Delivery.OnProgramme!.LearningSupport!);

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport,(actual,expected) => 
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate
            ))), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_Price_Changes()
    {
        var fixture = new Fixture();

        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedEpisodeKey = Guid.NewGuid();
        var expectedAgeAtStartOfLearning = _fixture.Create<int>();
        var expectedCosts = fixture.Create<List<UpdateLearnerApiPutResponse.EpisodePrice>>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices },
            AgeAtStartOfLearning = expectedAgeAtStartOfLearning,
            LearningEpisodeKey = expectedEpisodeKey,
            Prices = expectedCosts
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SavePricesApiPatchRequest>(
            r => 
                r.Data.ApprenticeshipEpisodeKey == expectedEpisodeKey &&
                r.Data.AgeAtStartOfLearning == expectedAgeAtStartOfLearning &&
                r.Data.Prices.HasEquivalentItems(expectedCosts, (actual, expected) =>
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate &&
                actual.TrainingPrice == expected.TrainingPrice &&
                actual.EndPointAssessmentPrice == expected.EndPointAssessmentPrice &&
                actual.TotalPrice == expected.TotalPrice
            ))), Times.Once);
    }

    private static void MockLearningApiResponse(
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

    private static bool Matches(SaveMathsAndEnglishRequest request, List<MathsAndEnglish> courses)
    {
        return request.Count == courses.Count &&
               request.All(r => courses.Any(c => c.StartDate == r.StartDate &&
                                                 c.EndDate == r.EndDate &&
                                                 c.Course == r.Course &&
                                                 c.Amount == r.Amount &&
                                                 c.WithdrawalDate == r.WithdrawalDate &&
                                                 c.PriorLearningAdjustment == r.PriorLearningAdjustmentPercentage &&
                                                 c.CompletionDate == r.ActualEndDate));
    }
}

internal static class ListExtensions
{
    public static bool HasEquivalentItems<TSource, TTarget>(
        this IEnumerable<TSource> source,
        IEnumerable<TTarget> target,
        Func<TSource, TTarget, bool> comparison)
    {
        if (source is null && target is null)
            return true;

        if (source is null || target is null)
            return false;

        if (source.Count() != target.Count())
            return false;

        return source.All(sourceItem => target.Any(targetItem => comparison(sourceItem, targetItem)));
    }
}