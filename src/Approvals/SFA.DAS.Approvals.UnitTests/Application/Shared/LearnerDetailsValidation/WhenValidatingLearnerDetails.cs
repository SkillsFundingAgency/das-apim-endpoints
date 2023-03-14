using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Shared.LearnerDetailsValidation
{
    public class WhenValidatingLearnerDetails
    {
        private Mock<ILearnerVerificationApiClient<LearnerVerificationApiConfiguration>> _learnerVerificationApiClient;
        private Fixture _fixture;
        private LearnerDetailsValidator _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _learnerVerificationApiClient = new Mock<ILearnerVerificationApiClient<LearnerVerificationApiConfiguration>>();
            _sut = new LearnerDetailsValidator(_learnerVerificationApiClient.Object);
        }

        [Test]
        public async Task Then_Return_A_LearnerVerificationResponse()
        {
            //Arrange
            var request = _fixture.Create<ValidateLearnerDetailsRequest>();
            var expectedResponse = _fixture.Create<LearnerVerificationResponse>();
            _learnerVerificationApiClient
                .Setup(x => x.Get<LearnerVerificationResponse>(It.IsAny<GetVerifyLearnerRequest>()))
                .ReturnsAsync(expectedResponse);

            //Act
            var response = await _sut.Validate(request);

            //Assert
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}