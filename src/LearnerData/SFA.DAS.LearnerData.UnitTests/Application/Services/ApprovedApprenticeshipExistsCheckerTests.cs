using System.Net;
using AutoFixture;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services
{
    [TestFixture]
    public class ApprovedApprenticeshipExistsCheckerTests
    {
        private readonly Fixture _fixture = new();
        private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
        private ApprovedApprenticeshipExistsChecker _sut;

        [SetUp]
        public void Setup()
        {
            _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
            _sut = new ApprovedApprenticeshipExistsChecker(_learningApiClient.Object);
        }

        [Test]
        public async Task Then_Returns_True_When_The_Inner_Api_Returns_Ok()
        {
            // Arrange
            _learningApiClient.Setup(x => x.Head(It.IsAny<CheckApprovedApprenticeshipExistsRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            // Act
            var result = await _sut.Exists(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<DateTime>());

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Then_Returns_False_When_The_Inner_Api_Returns_Not_Found()
        {
            // Arrange
            _learningApiClient.Setup(x => x.Head(It.IsAny<CheckApprovedApprenticeshipExistsRequest>()))
                .ReturnsAsync(HttpStatusCode.NotFound);

            // Act
            var result = await _sut.Exists(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<DateTime>());

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Then_Builds_The_Request_From_The_Given_Parameters()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var uln = _fixture.Create<string>();
            var standardCode = _fixture.Create<int>();
            var startDate = _fixture.Create<DateTime>();

            CheckApprovedApprenticeshipExistsRequest? capturedRequest = null;
            _learningApiClient.Setup(x => x.Head(It.IsAny<CheckApprovedApprenticeshipExistsRequest>()))
                .Callback<IHeadApiRequest>(r => capturedRequest = (CheckApprovedApprenticeshipExistsRequest)r)
                .ReturnsAsync(HttpStatusCode.NotFound);

            // Act
            await _sut.Exists(ukprn, uln, standardCode, startDate);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Ukprn.Should().Be(ukprn);
            capturedRequest.Uln.Should().Be(uln);
            capturedRequest.TrainingCode.Should().Be(standardCode.ToString());
            capturedRequest.StartDate.Should().Be(startDate);
            capturedRequest.IsApproved.Should().BeTrue();
        }
    }
}
