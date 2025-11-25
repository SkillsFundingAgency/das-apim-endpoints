using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
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
        var expectedCompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.First().CompletionDate;

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
        var expectedCompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.First().CompletionDate;
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
            .SelectMany(x => x.LearningSupport.Select(ls => new LearningSupportUpdatedDetails
            {
                StartDate = ls.StartDate,
                EndDate = ls.EndDate
            }))
            .ToList();

        expectedLearningSupport.AddRange(command.UpdateLearnerRequest.Delivery.OnProgramme!.First().LearningSupport!
            .Select(ls => new LearningSupportUpdatedDetails
            {
                StartDate = ls.StartDate,
                EndDate = ls.EndDate
            }));

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
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport, (actual, expected) =>
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate
            ))), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_LearningSupport_Changes_OnCompletion()
    {
        var startDate = new DateTime(2024, 8, 1);
        var completionDate = startDate.AddYears(1);

        var command = CreateLearnerCommandWithLearningSupport(startDate, completionDate: startDate.AddYears(1));

        var expectedLearningSupport = new List<LearningSupportUpdatedDetails>
        {
            new LearningSupportUpdatedDetails
            {
                StartDate = startDate,
                EndDate = completionDate
            }
        };

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        await _sut.Handle(command, CancellationToken.None);

        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport, (actual, expected) =>
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate
            ))), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_LearningSupport_Changes_OnWithdrawal()
    {
        var startDate = new DateTime(2024, 8, 1);
        var withdrawalDate = startDate.AddYears(1);

        var command = CreateLearnerCommandWithLearningSupport(startDate, withdrawalDate: startDate.AddYears(1));

        var expectedLearningSupport = new List<LearningSupportUpdatedDetails>
        {
            new LearningSupportUpdatedDetails
            {
                StartDate = startDate,
                EndDate = withdrawalDate
            }
        };

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        await _sut.Handle(command, CancellationToken.None);

        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport, (actual, expected) =>
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

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_ExpectedEndDate_Changes()
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
    public async Task Then_Learner_Is_Updated_Successfully_With_PersonalDetails_Changes()
    {
        var fixture = new Fixture();

        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        var expectedEpisodeKey = Guid.NewGuid();
        var expectedAgeAtStartOfLearning = _fixture.Create<int>();
        var expectedCosts = fixture.Create<List<UpdateLearnerApiPutResponse.EpisodePrice>>();

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.PersonalDetails },
            AgeAtStartOfLearning = expectedAgeAtStartOfLearning,
            LearningEpisodeKey = expectedEpisodeKey,
            Prices = expectedCosts
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<WithdrawApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(It.Is<UpdateLearningApiPutRequest>(
                r => r.Data.Learner.FirstName == command.UpdateLearnerRequest.Learner.FirstName
                && r.Data.Learner.LastName == command.UpdateLearnerRequest.Learner.LastName
                && r.Data.Learner.EmailAddress == command.UpdateLearnerRequest.Learner.Email
                )), Times.Once);

        _earningsApiClient.Invocations.Count.Should().Be(0);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_Withdrawal()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        command.UpdateLearnerRequest.Delivery.OnProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme.Take(1).ToList();

        var expectedWithdrawalDate = command.UpdateLearnerRequest.Delivery.OnProgramme.First().WithdrawalDate;

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.Withdrawal }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<WithdrawApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Delivery.WithdrawalDate == expectedWithdrawalDate)), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<WithdrawApiPatchRequest>(
            r => r.Data.WithdrawalDate == expectedWithdrawalDate)), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_Reverse_Withdrawal()
    {
        // Arrange
        var command = _fixture.Create<UpdateLearnerCommand>();
        command.UpdateLearnerRequest.Delivery.OnProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme.Take(1).ToList();
        var expectedWithdrawalDate = command.UpdateLearnerRequest.Delivery.OnProgramme.First().WithdrawalDate;

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.ReverseWithdrawal }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<ReverseWithdrawalApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.Is<UpdateLearningApiPutRequest>(r => r.Data.Delivery.WithdrawalDate == expectedWithdrawalDate)), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.IsAny<ReverseWithdrawalApiPatchRequest>()), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_EnglishAndMaths_LearningSupport_Changes_OnCompletion()
    {
        var startDate = new DateTime(2024, 8, 1);
        var completionDate = startDate.AddYears(1);

        var command = CreateEnglishAndMathsLearnerCommandWithLearningSupport(startDate, completionDate: completionDate);

        var expectedLearningSupport = new List<LearningSupportUpdatedDetails>
        {
            new LearningSupportUpdatedDetails
            {
                StartDate = startDate,
                EndDate = completionDate
            }
        };

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        await _sut.Handle(command, CancellationToken.None);

        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport, (actual, expected) =>
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate
            ))), Times.Once);
    }

    [Test]
    public async Task Then_Learner_Is_Updated_Successfully_With_EnglishAndMaths_LearningSupport_Changes_OnWithdrawal()
    {
        var startDate = new DateTime(2024, 8, 1);
        var withdrawalDate = startDate.AddYears(1);

        var command = CreateEnglishAndMathsLearnerCommandWithLearningSupport(startDate, withdrawalDate: withdrawalDate);

        var expectedLearningSupport = new List<LearningSupportUpdatedDetails>
        {
            new LearningSupportUpdatedDetails
            {
                StartDate = startDate,
                EndDate = withdrawalDate
            }
        };

        MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
        {
            Changes = { UpdateLearnerApiPutResponse.LearningUpdateChanges.LearningSupport }
        }, HttpStatusCode.OK);

        _earningsApiClient.Setup(x => x.Patch(It.IsAny<SaveCompletionApiPatchRequest>()))
            .Returns(Task.CompletedTask);

        await _sut.Handle(command, CancellationToken.None);

        _learningApiClient.Verify(x =>
            x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                It.IsAny<UpdateLearningApiPutRequest>()), Times.Once);

        _earningsApiClient.Verify(x => x.Patch(It.Is<SaveLearningSupportApiPutRequest>(
            r => r.Data.HasEquivalentItems(expectedLearningSupport, (actual, expected) =>
                actual.StartDate == expected.StartDate &&
                actual.EndDate == expected.EndDate
            ))), Times.Once);
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

    private static bool Matches(SaveMathsAndEnglishRequest request, List<MathsAndEnglish> courses)
    {
        return request.Count == courses.Count &&
               request.All(r => courses.Any(c => c.StartDate == r.StartDate &&
                                                 c.EndDate == r.EndDate &&
                                                 c.Course == r.Course &&
                                                 c.Amount == r.Amount &&
                                                 c.WithdrawalDate == r.WithdrawalDate &&
                                                 c.PriorLearningPercentage == r.PriorLearningAdjustmentPercentage &&
                                                 c.CompletionDate == r.ActualEndDate));
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