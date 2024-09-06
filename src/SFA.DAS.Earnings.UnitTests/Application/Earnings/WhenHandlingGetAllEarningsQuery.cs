using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
    public class WhenHandlingGetAllEarningsQuery
    {
        private GetAllEarningsQueryTestFixture _testFixture;

        [SetUp]
        public async Task SetUp()
        {
            // Arrange
            _testFixture = new GetAllEarningsQueryTestFixture();

            // Act
            await _testFixture.CallSubjectUnderTest();
        }

        [Test]
        public void ThenCallsApprenticeshipsApi()
        {
            //Assert
            _testFixture.MockApprenticeshipsApiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
        }

        [Test]
        public void ThenCallsEarningsApi()
        {
            //Assert
            _testFixture.MockEarningsApiClient.Verify(x => x.Get<GetFm36DataResponse>(It.Is<GetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
        }

        [Test]
        public void ThenReturnsFM36LearnerIdentifiers()
        {
            // Assert
            _testFixture.Result.Should().NotBeNull();

            foreach (var apprenticeship in _testFixture.ApprenticeshipsResponse.Apprenticeships)
            {
                _testFixture.Result.FM36Learners.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == "9999999999");
            }
        }
    }

}