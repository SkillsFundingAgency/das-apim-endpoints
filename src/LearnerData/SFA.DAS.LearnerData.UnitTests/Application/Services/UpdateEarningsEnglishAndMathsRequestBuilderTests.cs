using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Helpers;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class UpdateEarningsEnglishAndMathsRequestBuilderTests
    {
        private readonly Fixture _fixture = new();
        private UpdateEarningsEnglishAndMathsRequestBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new UpdateEarningsEnglishAndMathsRequestBuilder();
        }

        [Test]
        public void Build_Should_Map_EnglishAndMathsItems_And_LearningKey()
        {
            // Arrange
            var learnAimRef = _fixture.Create<string>();
            var command = _fixture.Create<UpdateLearnerCommand>();
            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.ForEach(x =>
            {
                x.LearnAimRef = learnAimRef;
                x.CompletionDate = null;
                x.WithdrawalDate = null;
                x.PauseDate = x.ActualEndDate;
                x.EndDate = x.ActualEndDate.Value.AddDays(30);
            });
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            putRequest.Data.MathsAndEnglishCourses.ForEach(x => x.LearnAimRef = learnAimRef);
            var response = _fixture.Create<UpdateLearnerApiPutResponse>();

            // Act
            var result = _sut.Build(command, response, putRequest);

            // Assert
            var expectedItems = putRequest.Data.MathsAndEnglishCourses.Select(x => new EnglishAndMathsItem
            {
                StartDate = x.StartDate,
                EndDate = x.PlannedEndDate,
                Course = x.Course,
                LearnAimRef = x.LearnAimRef,
                Amount = x.Amount,
                WithdrawalDate = x.WithdrawalDate,
                PriorLearningAdjustmentPercentage = x.PriorLearningPercentage,
                ActualEndDate = x.CompletionDate,
                PauseDate = x.PauseDate,
                PeriodsInLearning = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Where(e => e.LearnAimRef == x.LearnAimRef)
                    .Select(y => new PeriodInLearningItem
                    {
                        StartDate = y.StartDate,
                        EndDate = DateTimeHelper.EarliestOf(y.EndDate, y.ActualEndDate, y.CompletionDate, y.WithdrawalDate) ?? y.EndDate,
                        OriginalExpectedEndDate = y.EndDate
                    })
                    .OrderBy(p => p.StartDate)
                    .ToList()
            }).ToList();

            result.PutUrl.Should().Be($"learning/{command.LearningKey}/english-and-maths");
            result.Data.EnglishAndMaths.Should().BeEquivalentTo(expectedItems);
        }
    }
}