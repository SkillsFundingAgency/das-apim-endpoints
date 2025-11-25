using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using FluentAssertions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LearnerData.UnitTests.Application.UpdateLearner
{
    internal class WhenHandlingUpdateLearnerCommandWithBreaksInLearning
    {
        private readonly Fixture _fixture = new();

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
        private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
        private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
        private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
        private UpdateLearnerCommandHandler _sut;


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
        public async Task With_No_Change_In_Price_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            _learningApiClient.Verify(x =>
                    x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                        It.IsAny<UpdateLearningApiPutRequest>()),
                Times.Once);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            var expectedBreakInLearning = new BreakInLearning
            {
                StartDate = command.UpdateLearnerRequest.Delivery.OnProgramme[0].ActualEndDate!.Value.AddDays(1),
                EndDate = command.UpdateLearnerRequest.Delivery.OnProgramme[1].StartDate.AddDays(-1)
            };

            actualRequest.Data.OnProgramme.BreaksInLearning.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(expectedBreakInLearning);

            actualRequest.Data.OnProgramme.Costs.Should().HaveCount(1);

            var expected = command.UpdateLearnerRequest.Delivery.OnProgramme.First().Costs.First();
            actualRequest.Data.OnProgramme.Costs.First().Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task With_Change_In_Price_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(true);
            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            _learningApiClient.Verify(x =>
                x.PutWithResponseCode<UpdateLearningRequestBody, UpdateLearnerApiPutResponse>(
                    It.IsAny<UpdateLearningApiPutRequest>()),
                Times.Once);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            var expectedBreakInLearning = new BreakInLearning
            {
                StartDate = command.UpdateLearnerRequest.Delivery.OnProgramme[0].ActualEndDate!.Value.AddDays(1),
                EndDate = command.UpdateLearnerRequest.Delivery.OnProgramme[1].StartDate.AddDays(-1)
            };

            actualRequest.Data.OnProgramme.BreaksInLearning.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(expectedBreakInLearning);

            actualRequest.Data.OnProgramme.Costs.Should().HaveCount(2);

            var expectedFirstCost = command.UpdateLearnerRequest.Delivery.OnProgramme[0].Costs.First();
            var expectedSecondCost = command.UpdateLearnerRequest.Delivery.OnProgramme[1].Costs.First();

            actualRequest.Data.OnProgramme.Costs[0].Should().BeEquivalentTo(expectedFirstCost, opts =>
                opts.Excluding(c => c.FromDate));

            actualRequest.Data.OnProgramme.Costs[1].Should().BeEquivalentTo(expectedSecondCost, opts =>
                opts.Excluding(c => c.FromDate));
        }

        [Test]
        public async Task With_WithdrawalDate_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var withdrawalDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().WithdrawalDate = withdrawalDate;

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            actualRequest.Data.Delivery.WithdrawalDate.Should().Be(withdrawalDate);
        }

        [Test]
        public async Task With_CompletionDate_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var completionDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().CompletionDate = completionDate;

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            actualRequest.Data.Learner.CompletionDate.Should().Be(completionDate);
        }

        [Test]
        public async Task With_ExpectedEndDate_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var expectedEndDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().ExpectedEndDate = expectedEndDate;

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            actualRequest.Data.OnProgramme.ExpectedEndDate.Should().Be(expectedEndDate);
        }

        [Test]
        public async Task With_PauseDate_Then_Learning_Is_Updated()
        {
            var fixture = new Fixture();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var pauseDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().PauseDate = pauseDate;

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse(), HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            var actualRequest = _learningApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateLearningApiPutRequest>()
                .Single();

            // Assert
            actualRequest.Data.OnProgramme.PauseDate.Should().Be(pauseDate);
        }

        [Test]
        public async Task When_LearningResponse_Indicates_BreakInLearningUpdated_Then_Earnings_Is_Updated()
        {
            var fixture = new Fixture();
            var episodeKey = fixture.Create<Guid>();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
            {
                Changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
                {
                    UpdateLearnerApiPutResponse.LearningUpdateChanges.BreaksInLearningUpdated
                },
                LearningEpisodeKey = episodeKey
            }, HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Capture the actual request sent to the earnings API
            var actualRequest = _earningsApiClient.Invocations
                .Select(i => i.Arguments[0])
                .OfType<UpdateBreaksInLearningApiPatchRequest>()
                .Single();

            // Assert
            var expectedBreakInLearning = new BreakInLearning
            {
                StartDate = command.UpdateLearnerRequest.Delivery.OnProgramme[0].ActualEndDate!.Value.AddDays(1),
                EndDate = command.UpdateLearnerRequest.Delivery.OnProgramme[1].StartDate.AddDays(-1)
            };

            actualRequest.Data.EpisodeKey.Should().Be(episodeKey);

            actualRequest.Data.BreaksInLearning.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(expectedBreakInLearning);
        }

        [Test]
        public async Task When_LearningResponse_Indicates_BreakInLearningStarted_Then_Earnings_Is_Updated()
        {
            var fixture = new Fixture();
            var episodeKey = fixture.Create<Guid>();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
            var pauseDate = fixture.Create<DateTime>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Last().PauseDate = pauseDate;

            MockLearningApiResponse(_learningApiClient, new UpdateLearnerApiPutResponse
            {
                Changes = new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
                {
                    UpdateLearnerApiPutResponse.LearningUpdateChanges.BreakInLearningStarted
                },
                LearningEpisodeKey = episodeKey
            }, HttpStatusCode.OK);

            // Act
            await _sut.Handle(command, CancellationToken.None);

            //Assert
            _earningsApiClient.Verify(x =>
                x.Patch(It.Is<PauseApiPatchRequest>(r =>
                    r.Data.PauseDate == pauseDate)), Times.Once);
        }

        private UpdateLearnerCommand CreateLearnerWithBreaksInLearning(bool withPriceChange)
        {
            var command = _fixture.Create<UpdateLearnerCommand>();
            command.UpdateLearnerRequest.Delivery.OnProgramme.Clear();

            var standardCode = _fixture.Create<int>();
            var agreementId = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime>();
            var pauseDate = startDate.AddMonths(6);
            var resumeDate = pauseDate.AddMonths(6);

            var initialCosts = new List<CostDetails>
            {
                new CostDetails
                {
                    FromDate = startDate,
                    TrainingPrice = _fixture.Create<int>(),
                    EpaoPrice = _fixture.Create<int>(),
                }
            };

            var resumeCosts = withPriceChange ?
                [
                    new CostDetails
                    {
                        FromDate = resumeDate,
                        TrainingPrice = initialCosts.First().TrainingPrice + 1000,
                        EpaoPrice = initialCosts.First().EpaoPrice + 1000,
                    }
                ]
                : initialCosts;

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddYears(2),
                ActualEndDate = pauseDate,
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                Costs = initialCosts,
                LearningSupport = []
            });

            command.UpdateLearnerRequest.Delivery.OnProgramme.Add(new OnProgrammeRequestDetails
            {
                StartDate = resumeDate,
                ExpectedEndDate = resumeDate.AddYears(2),
                StandardCode = standardCode,
                AgreementId = agreementId,
                PauseDate = null,
                WithdrawalDate = null,
                CompletionDate = null,
                Costs = resumeCosts,
                LearningSupport = []
            });

            return command;
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
}
