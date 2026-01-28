using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Helpers;
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

        [TestCase(true)]
        [TestCase(false)]
        public async Task Build_Should_Map_Payload_Without_FundingBandUpdate(bool completion)
        {
            // Arrange
            var command = _fixture.Create<UpdateLearnerCommand>();
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            var response = _fixture.Build<UpdateLearnerApiPutResponse>()
                                   .With(r => r.Changes, new List<UpdateLearnerApiPutResponse.LearningUpdateChanges>())
                                   .Create();
            var agreementId = command.UpdateLearnerRequest.Delivery.OnProgramme.First().AgreementId;

            if (!completion)
            {
                putRequest.Data.Learner.CompletionDate = null;
                command.UpdateLearnerRequest.Delivery.OnProgramme.ForEach(x => x.CompletionDate = null);
            }
                

            // Act
            var result = await _sut.Build(command, response, putRequest);

            // Assert
            result.PutUrl.Should().Be($"learning/{command.LearningKey}/on-programme");
            result.Data.CompletionDate.Should().Be(putRequest.Data.Learner.CompletionDate);
            result.Data.WithdrawalDate.Should().Be(putRequest.Data.Delivery.WithdrawalDate);
            result.Data.PauseDate.Should().Be(putRequest.Data.OnProgramme.PauseDate);
            result.Data.ApprenticeshipEpisodeKey.Should().Be(response.LearningEpisodeKey);
            result.Data.DateOfBirth.Should().Be(putRequest.Data.Learner.DateOfBirth);

            result.Data.Prices.Should().BeEquivalentTo(response.Prices.Select(x => new PriceItem
            {
                Key = x.Key,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TrainingPrice = x.TrainingPrice,
                EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                TotalPrice = x.TotalPrice
            }));

            if (completion)
            {
                result.Data.PeriodsInLearning.Should().BeEquivalentTo(
                    command.UpdateLearnerRequest.Delivery.OnProgramme
                        .Where(x => x.AgreementId == agreementId)
                        .Select(x => new PeriodInLearningItem
                    {
                        StartDate = x.StartDate,
                        EndDate = DateTimeHelper.EarliestOf(x.ExpectedEndDate, x.PauseDate, x.WithdrawalDate)!.Value,
                        OriginalExpectedEndDate = x.ExpectedEndDate
                    }));
            }
            else
            {
                result.Data.PeriodsInLearning.Should().BeEquivalentTo(
                    command.UpdateLearnerRequest.Delivery.OnProgramme
                        .Where(x => x.AgreementId == agreementId)
                        .Select(x => new PeriodInLearningItem
                    {
                        StartDate = x.StartDate,
                        EndDate = DateTimeHelper.EarliestOf(x.ExpectedEndDate, x.PauseDate, x.WithdrawalDate, x.ActualEndDate)!.Value,
                        OriginalExpectedEndDate = x.ExpectedEndDate
                    }));
            }

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
                                   .With(r => r.Changes, [UpdateLearnerApiPutResponse.LearningUpdateChanges.Prices])
                                   .Create();

            var expectedFundingBand = 9000;
            _coursesApiClient.Setup(c => c.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                             .ReturnsAsync(new StandardDetailResponse
                             {
                                 ApprenticeshipFunding =
                                 [
                                     new ApprenticeshipFunding
                                     {
                                         EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                         EffectiveTo = DateTime.UtcNow.AddMonths(1),
                                         MaxEmployerLevyCap = expectedFundingBand
                                     }
                                 ]
                             });

            // Act
            var result = await _sut.Build(command, response, putRequest);

            // Assert
            result.Data.FundingBandMaximum.Should().Be(expectedFundingBand);
            result.Data.IncludesFundingBandMaximumUpdate.Should().BeTrue();
        }

        [Test]
        public async Task Build_Should_Set_CareDetails()
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
            result.PutUrl.Should().Be($"learning/{command.LearningKey}/on-programme");
            result.Data.Care.HasEHCP.Should().Be(putRequest.Data.Learner.Care.HasEHCP);
            result.Data.Care.IsCareLeaver.Should().Be(putRequest.Data.Learner.Care.IsCareLeaver);
            result.Data.Care.CareLeaverEmployerConsentGiven.Should().Be(putRequest.Data.Learner.Care.CareLeaverEmployerConsentGiven);

        }

    }
}