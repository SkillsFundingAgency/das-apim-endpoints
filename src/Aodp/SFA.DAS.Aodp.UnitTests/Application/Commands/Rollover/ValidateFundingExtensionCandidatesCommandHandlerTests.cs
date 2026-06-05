using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Rollover
{
    [TestFixture]
    public class ValidateFundingExtensionCandidatesCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private ValidateFundingExtensionCandidatesCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new ValidateFundingExtensionCandidatesCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = _fixture.Create<ValidateFundingExtensionCandidatesCommand>();

            var apiResponse = new ApiResponse<ValidateFundingExtensionCandidatesCommandResponse>(
               new ValidateFundingExtensionCandidatesCommandResponse
               {
                   TotalCandidates = 1,
                   FailedCandidateCount = 0,
                   IsValid = true
               },
               HttpStatusCode.OK,
               string.Empty);

            _apiClientMock
                .Setup(c => c.PostWithResponseCode<ValidateFundingExtensionCandidatesCommandResponse>(
                    It.IsAny<ValidateFundingExtensionCandidatesApiRequest>(), true))
                .ReturnsAsync(apiResponse);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.TotalCandidates, Is.EqualTo(1));
            Assert.That(result.Value.FailedCandidateCount, Is.EqualTo(0));
            Assert.That(result.Value.IsValid, Is.True);
        }


        [Test]
        public async Task Handle_WhenApiClientThrows_ShouldReturnFailureResponse()
        {
            // Arrange
            var command = _fixture.Create<ValidateFundingExtensionCandidatesCommand>();

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<ValidateFundingExtensionCandidatesCommandResponse>(
                    It.IsAny<ValidateFundingExtensionCandidatesApiRequest>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("API failure"));

            _apiClientMock.Verify(x =>
                x.PostWithResponseCode<ValidateFundingExtensionCandidatesCommandResponse>(
                    It.IsAny<ValidateFundingExtensionCandidatesApiRequest>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_ShouldSendCorrectApiRequest()
        {
            // Arrange
            var command = _fixture.Create<ValidateFundingExtensionCandidatesCommand>();

            ValidateFundingExtensionCandidatesApiRequest? capturedRequest = null;

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<ValidateFundingExtensionCandidatesCommandResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .Callback<IPostApiRequest, bool>((req, _) =>
                {
                    capturedRequest = req as ValidateFundingExtensionCandidatesApiRequest;
                })
                .ReturnsAsync(new ApiResponse<ValidateFundingExtensionCandidatesCommandResponse>(
                    new ValidateFundingExtensionCandidatesCommandResponse
                    {
                        TotalCandidates = 1,
                        FailedCandidateCount = 0,
                        IsValid = true
                    },
                    HttpStatusCode.OK,
                    string.Empty));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Data, Is.EqualTo(command));
        }

    }
}
