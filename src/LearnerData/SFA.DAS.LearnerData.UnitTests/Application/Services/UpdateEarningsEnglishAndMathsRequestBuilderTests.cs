using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
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
            var command = _fixture.Create<UpdateLearnerCommand>();
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            var response = _fixture.Create<UpdateLearnerApiPutResponse>();

            // Act
            var result = _sut.Build(command, response, putRequest);

            // Assert
            var expectedItems = putRequest.Data.MathsAndEnglishCourses.Select(x => new EnglishAndMathsItem
            {
                StartDate = x.StartDate,
                EndDate = x.PlannedEndDate,
                Course = x.Course,
                Amount = x.Amount,
                WithdrawalDate = x.WithdrawalDate,
                PriorLearningAdjustmentPercentage = x.PriorLearningPercentage,
                ActualEndDate = x.CompletionDate,
                PauseDate = x.PauseDate
            }).ToList();

            result.PutUrl.Should().Be($"learning/{command.LearningKey}/english-and-maths");
            result.Data.EnglishAndMaths.Should().BeEquivalentTo(expectedItems);
        }
    }
}