using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class UpdateEarningsOnProgrammeRequestBuilderTests
    {
        private readonly Fixture _fixture = new();
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
        private UpdateEarningsOnProgrammeRequestBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            _sut = new UpdateEarningsOnProgrammeRequestBuilder(_coursesApiClient.Object);
        }

        [Test]
        public async Task Build_Should_Map_Payload_Without_FundingBandUpdate()
        {
            // Arrange
            var command = _fixture.Create<UpdateLearnerCommand>();
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            var response = _fixture.Build<UpdateLearnerApiPutResponse>()
                                   .With(r => r.Changes, new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>())
                                   .Create();

            // Act
            var result = await _sut.Build(command, response, putRequest);

            // Assert
            //result.LearningKey.Should().Be(command.LearningKey);
            result.Data.CompletionDate.Should().Be(putRequest.Data.Learner.CompletionDate);
            result.Data.WithdrawalDate.Should().Be(putRequest.Data.Delivery.WithdrawalDate);
            result.Data.PauseDate.Should().Be(putRequest.Data.OnProgramme.PauseDate);
            result.Data.ApprenticeshipEpisodeKey.Should().Be(response.LearningEpisodeKey);
            result.Data.AgeAtStartOfLearning.Should().Be(response.AgeAtStartOfLearning);

            result.Data.Prices.Should().BeEquivalentTo(response.Prices.Select(x => new PriceItem
            {
                Key = x.Key,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TrainingPrice = x.TrainingPrice,
                EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                TotalPrice = x.TotalPrice
            }));

            result.Data.BreaksInLearning.Should().BeEquivalentTo(
                putRequest.Data.OnProgramme.BreaksInLearning.Select(x => new BreakInLearningItem
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    PriorPeriodExpectedEndDate = x.PriorPeriodExpectedEndDate
                }));

            result.Data.FundingBandMaximum.Should().BeNull();
            result.Data.IncludesFundingBandMaximumUpdate.Should().BeFalse();
        }

        [Test]
        public async Task Build_Should_Set_FundingBandMaximum_When_Prices_Change()
        {
            // Arrange
            var command = _fixture.Create<UpdateLearnerCommand>();
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            var response = _fixture.Build<UpdateLearnerApiPutResponse>()
                                   .With(r => r.Changes, new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>
                                   {
                                       UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices
                                   })
                                   .Create();

            var expectedFundingBand = 9000;
            _coursesApiClient.Setup(c => c.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                             .ReturnsAsync(new StandardDetailResponse
                             {
                                 ApprenticeshipFunding = new List<ApprenticeshipFunding>
                                 {
                                     new ApprenticeshipFunding
                                     {
                                         EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                         EffectiveTo = DateTime.UtcNow.AddMonths(1),
                                         MaxEmployerLevyCap = expectedFundingBand
                                     }
                                 }
                             });

            // Act
            var result = await _sut.Build(command, response, putRequest);

            // Assert
            result.Data.FundingBandMaximum.Should().Be(expectedFundingBand);
            result.Data.IncludesFundingBandMaximumUpdate.Should().BeTrue();
        }
    }
}