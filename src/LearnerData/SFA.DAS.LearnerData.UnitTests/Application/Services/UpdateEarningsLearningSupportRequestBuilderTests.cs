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
    public class UpdateEarningsLearningSupportRequestBuilderTests
    {
        private readonly Fixture _fixture = new();
        private UpdateEarningsLearningSupportRequestBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new UpdateEarningsLearningSupportRequestBuilder();
        }

        [Test]
        public void Build_Should_Map_LearningSupportItems()
        {
            // Arrange
            var command = _fixture.Create<UpdateLearnerCommand>();
            var putRequest = _fixture.Create<UpdateLearningApiPutRequest>();
            var response = _fixture.Create<UpdateLearnerApiPutResponse>();

            // Act
            var result = _sut.Build(command, response, putRequest);

            // Assert
            var expected = putRequest.Data.LearningSupport.Select(x => new
                LearningSupportItem
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();

            result.Data.LearningSupport.Should().BeEquivalentTo(expected);
        }
    }
}