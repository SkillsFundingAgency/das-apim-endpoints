using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.UpdateLearner
{
    internal class WhenHandlingUpdateLearnerCommandWithBreaksInLearning
    {
        private readonly Fixture _fixture = new();

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
        private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
        private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
        private Mock<IUpdateLearningPutRequestBuilder> _updateLearningPutRequestBuilder;
        private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
        private UpdateLearnerCommandHandler _sut;


#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

        [SetUp]
        public void Setup()
        {
            _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
            _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
            _updateLearningPutRequestBuilder = new Mock<IUpdateLearningPutRequestBuilder>();
            _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
            _sut = new UpdateLearnerCommandHandler(
                _logger.Object,
                _learningApiClient.Object,
                _earningsApiClient.Object,
                _updateLearningPutRequestBuilder.Object,
                Mock.Of<ICoursesApiClient<CoursesApiConfiguration>>());
        }

        //todo: create a test to cover Learning Update based off the builder's response (mocked)
        



        [Test]
        public async Task When_LearningResponse_Indicates_BreakInLearningStarted_Then_Earnings_Is_Updated()
        {
            var fixture = new Fixture();
            var episodeKey = fixture.Create<Guid>();

            // Arrange
            var command = CreateLearnerWithBreaksInLearning(false);
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
        public async Task When_LearningResponse_Indicates_BreakInLearningRemoved_Then_Earnings_Is_Updated()
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

            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Clear();

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

        protected void MockEmptyLearningApiResponse()
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

        private T CaptureRequest<T>(Mock mock) where T : class
    => mock.Invocations
           .Select(i => i.Arguments[0])
           .OfType<T>()
           .Single();
    }
}
