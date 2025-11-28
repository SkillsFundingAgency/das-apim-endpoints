using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Features;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
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
    private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
    private Mock<IUpdateLearningPutRequestBuilder> _updateLearningPutRequestBuilder;
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
        _updateLearningPutRequestBuilder = new Mock<IUpdateLearningPutRequestBuilder>();
        _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
        _sut = new UpdateLearnerCommandHandler(
            _logger.Object,
            _learningApiClient.Object,
            _earningsApiClient.Object,
            _updateLearningPutRequestBuilder.Object,
            _coursesApiClient.Object);
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
    public async Task Then_Earnings_Is_Updated_With_CompletionDate()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.CompletionDate }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveCompletionApiPatchRequest>(
                r => r.Data.CompletionDate == apiPutRequest.Data.Learner.CompletionDate)),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_MathsAndEnglish()
    {
        // Arrange
        var command = CreateUpdateLearnerCommand();
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglish }
        }, HttpStatusCode.OK);

        _earningsApiClient
            .Setup(x => x.Patch(It.IsAny<SaveMathsAndEnglishApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveMathsAndEnglishApiPatchRequest>(
                r => Matches(r.Data, apiPutRequest.Data.MathsAndEnglishCourses))),
            Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_LearningSupport_Changes()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        command.UpdateLearnerRequest.Delivery.OnProgramme.ForEach(x=> x.CompletionDate = null);
        command.UpdateLearnerRequest.Delivery.OnProgramme.ForEach(x=> x.WithdrawalDate = null);
        command.UpdateLearnerRequest.Delivery.OnProgramme.ForEach(x => x.PauseDate = null);
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths.ForEach(x =>
        {
            x.CompletionDate = null;
            x.WithdrawalDate = null;
        });

        // Clone expected learning support with original EndDates
        var expectedLearningSupport = command.UpdateLearnerRequest.Delivery.EnglishAndMaths
            .SelectMany(x => x.LearningSupport.Select(ls => new LearningSupportPaymentDetail
            {
                StartDate = ls.StartDate,
                EndDate = ls.EndDate
            }))
            .ToList();

        expectedLearningSupport.AddRange(command.UpdateLearnerRequest.Delivery.OnProgramme.SelectMany(x => x.LearningSupport)
            .Select(ls => new LearningSupportPaymentDetail
            {
                StartDate = ls.StartDate,
                EndDate = ls.EndDate
            }));

        var apiPutRequest = MockLearningPutRequestBuilder(command);

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch<SaveLearningSupportRequest>(It.IsAny<SaveLearningSupportApiPutRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert

        _earningsApiClient.Verify(x => x.Patch<SaveLearningSupportRequest>(
            It.Is<SaveLearningSupportApiPutRequest>(r =>
                r.Data.All(actual =>
                    apiPutRequest.Data.LearningSupport.Any(expected =>
                        actual.StartDate == expected.StartDate &&
                        actual.EndDate == expected.EndDate
                    )))), Times.Once);
    }


    [Test]
    public async Task Then_Earnings_Is_Updated_With_Price_Changes()
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

        var coursesApiResponse = _fixture.Create<StandardDetailResponse>();
        _coursesApiClient.Setup(x => x.Get<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(x => x.Id == command.UpdateLearnerRequest.Delivery.OnProgramme.First().StandardCode.ToString())))
            .ReturnsAsync(coursesApiResponse);

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

    [Test]
    public async Task Then_Earnings_Is_Updated_With_ExpectedEndDate_Changes()
    {
        var fixture = new Fixture();

        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedEpisodeKey = Guid.NewGuid();
        var expectedAgeAtStartOfLearning = _fixture.Create<int>();
        var expectedCosts = fixture.Create<List<UpdateLearnerApiPutResponse.EpisodePrice>>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate },
            AgeAtStartOfLearning = expectedAgeAtStartOfLearning,
            LearningEpisodeKey = expectedEpisodeKey,
            Prices = expectedCosts
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        var coursesApiResponse = _fixture.Create<StandardDetailResponse>();
        _coursesApiClient.Setup(x => x.Get<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(x => x.Id == command.UpdateLearnerRequest.Delivery.OnProgramme.First().StandardCode.ToString())))
            .ReturnsAsync(coursesApiResponse);

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


    [Test]
    public async Task Then_Earnings_Is_Updated_With_FundingBand_Changes()
    {
        var fixture = new Fixture();

        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedEpisodeKey = Guid.NewGuid();
        var expectedAgeAtStartOfLearning = _fixture.Create<int>();
        var expectedCosts = fixture.Create<List<UpdateLearnerApiPutResponse.EpisodePrice>>();
        var fundingBandMaximum = fixture.Create<int>();
        var coursesApiResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding =
            [
                new ApprenticeshipFunding
                {
                    EffectiveFrom = DateTime.MinValue,
                    EffectiveTo = DateTime.MaxValue,
                    MaxEmployerLevyCap = fundingBandMaximum
                }
            ]
        };

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices },
            AgeAtStartOfLearning = expectedAgeAtStartOfLearning,
            LearningEpisodeKey = expectedEpisodeKey,
            Prices = expectedCosts
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        _coursesApiClient.Setup(x => x.Get<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(x => x.Id == command.UpdateLearnerRequest.Delivery.OnProgramme.First().StandardCode.ToString())))
            .ReturnsAsync(coursesApiResponse);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SavePricesApiPatchRequest>(
            r =>
               r.Data.ApprenticeshipEpisodeKey == expectedEpisodeKey &&
               r.Data.FundingBandMaximum == fundingBandMaximum
               )), Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Not_Updated_With_PersonalDetails()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<WithdrawApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.Invocations.Count.Should().Be(0);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_Withdrawal()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var apiPutRequest = MockLearningPutRequestBuilder(command);

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<WithdrawApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        _earningsApiClient.Verify(x => x.Patch(It.Is<WithdrawApiPatchRequest>(
            r => r.Data.WithdrawalDate == apiPutRequest.Data.Delivery.WithdrawalDate)), Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_Reverse_Withdrawal()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.ReverseWithdrawal }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<ReverseWithdrawalApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _earningsApiClient.Verify(x => x.Patch(It.IsAny<ReverseWithdrawalApiPatchRequest>()), Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_BreakInLearningStarted()
    {
        var fixture = new Fixture();
        var episodeKey = fixture.Create<Guid>();

        // Arrange
        var command = fixture.Create<UpdateLearnerCommand>();
        var apiPutRequest = fixture.Create<UpdateLearningApiPutRequest>();
        _updateLearningPutRequestBuilder.Setup(x => x.Build(command)).Returns(apiPutRequest);

        MockLearningApiResponse(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningStarted, episodeKey);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x =>
            x.Patch(It.Is<PauseApiPatchRequest>(r =>
                r.Data.PauseDate == apiPutRequest.Data.OnProgramme.PauseDate)), Times.Once);
    }

    [Test]
    public async Task Then_Earnings_Is_Updated_With_BreakInLearningRemoved()
    {
        var fixture = new Fixture();
        var episodeKey = fixture.Create<Guid>();

        // Arrange
        var command = fixture.Create<UpdateLearnerCommand>();

        MockLearningApiResponse(UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningRemoved, episodeKey);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        //Assert
        _earningsApiClient.Verify(x =>
            x.Delete(It.IsAny<RemovePauseApiDeleteRequest>()), Times.Once);
    }
    
    [Test]
    public async Task Then_Earnings_Is_Updated_With_Maths_And_English_Withdrawal()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var apiPutRequest = _fixture.Create<UpdateLearningApiPutRequest>();
        _updateLearningPutRequestBuilder.Setup(x => x.Build(command)).Returns(apiPutRequest);

        MockLearningApiResponse(UpdateLearnerApiPutResponse.LearningUpdateChanges.MathsAndEnglishWithdrawal);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        //Assert
        _earningsApiClient.Verify(x =>
            x.Patch(It.Is<MathsAndEnglishWithdrawApiPatchRequest>(r =>
                r.Data.WithdrawalDate == apiPutRequest.Data.MathsAndEnglishCourses.First().WithdrawalDate && r.Data.Course == apiPutRequest.Data.MathsAndEnglishCourses.First().Course)), Times.Once);
    }

    protected void MockLearningApiResponse()
    {
        var responseBody = new UpdateLearnerApiPutResponse();
        var response = new ApiResponse<UpdateLearnerApiPutResponse>(responseBody, HttpStatusCode.OK, string.Empty);
        _learningApiClient.Setup(x =>
                x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.IsAny<UpdateLearningApiPutRequest>()))
            .ReturnsAsync(response);
    }

    protected void MockLearningApiResponse(UpdateLearnerApiPutResponse.LearningUpdateChanges changes, Guid? episodeKey = null)
    {
        var responseBody = new UpdateLearnerApiPutResponse
        {
            Changes = [changes],
            LearningEpisodeKey = episodeKey ?? Guid.Empty
        };

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

    private static bool Matches(SaveMathsAndEnglishRequest request, List<MathsAndEnglishDetails> courses)
    {
        return request.Count == courses.Count &&
               request.All(r => courses.Any(c => c.StartDate == r.StartDate &&
                                                 c.PlannedEndDate == r.EndDate &&
                                                 c.Course == r.Course &&
                                                 c.Amount == r.Amount &&
                                                 c.WithdrawalDate == r.WithdrawalDate &&
                                                 c.PriorLearningPercentage == r.PriorLearningAdjustmentPercentage &&
                                                 c.CompletionDate == r.ActualEndDate));
    }

    private UpdateLearnerCommand CreateUpdateLearnerCommand()
    {
        var command = _fixture.Create<UpdateLearnerCommand>();
        command.UpdateLearnerRequest.Delivery.OnProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme.Take(1).ToList();

        return command;
    }

    private UpdateLearnerCommand CreateLearnerCommandWithLearningSupport(DateTime startDate, DateTime? completionDate = null, DateTime? withdrawalDate = null)
    {
        var command = _fixture.Create<UpdateLearnerCommand>();
        var onProgramme = _fixture.Create<OnProgrammeRequestDetails>();

        onProgramme.Costs!.Clear();
        onProgramme.Costs.Add(new CostDetails
        {
            FromDate = startDate,
            TrainingPrice = 1000,
            EpaoPrice = 100
        });
        onProgramme.ExpectedEndDate = startDate.AddYears(2);
        onProgramme.CompletionDate = completionDate;
        onProgramme.WithdrawalDate = withdrawalDate;
        onProgramme.LearningSupport.Clear();
        onProgramme.LearningSupport.Add(new LearningSupportRequestDetails
        {
            StartDate = startDate,
            EndDate = startDate.AddYears(2)
        });
        onProgramme.PauseDate = null;

        command.UpdateLearnerRequest.Delivery.OnProgramme.Clear();
        command.UpdateLearnerRequest.Delivery.OnProgramme.Add(onProgramme);

        command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();

        return command;
    }

    private UpdateLearnerCommand CreateEnglishAndMathsLearnerCommandWithLearningSupport(DateTime startDate, DateTime? completionDate = null, DateTime? withdrawalDate = null)
    {
        var command = _fixture.Create<UpdateLearnerCommand>();

        command.UpdateLearnerRequest.Delivery.OnProgramme.ForEach(onProg =>
        {
            onProg.LearningSupport.Clear();
            onProg.CompletionDate = null;
            onProg.WithdrawalDate = null;
        });

        command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();
        command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Add(new MathsAndEnglish
        {
            CompletionDate = completionDate,
            WithdrawalDate = withdrawalDate,
            LearningSupport =
            [
                new LearningSupportRequestDetails
                {
                    StartDate = startDate,
                    EndDate = startDate.AddYears(2)
                }
            ]
        });

        return command;
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