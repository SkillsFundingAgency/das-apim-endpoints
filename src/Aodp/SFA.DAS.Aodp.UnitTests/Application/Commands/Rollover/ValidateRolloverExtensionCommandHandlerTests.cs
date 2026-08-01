using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Rollover
{
    [TestFixture]
    public class ValidateRolloverExtensionCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private ValidateRolloverExtensionCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new ValidateRolloverExtensionCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = _fixture.Create<ValidateRolloverExtensionCommand>();

            var apiResponse = new ApiResponse<ValidateRolloverExtensionCommandResponse>(
               new ValidateRolloverExtensionCommandResponse
               {
                   IsValid = true,
                   ValidationSuccessSummary = new FundingExtensionSummary 
                   {
                        TotalCandidatesCount = 25,
                        CandidatesExtendedInUploadCount = 4,
                        TotalCandidatesToBeExcludedCount = 14,
                        TotalCandidatesToBeExtendedCount = 8,
                        TotalCandidatesToBeReviewedCount = 3
                   }
               },
               HttpStatusCode.OK,
               string.Empty);

            _apiClientMock
                .Setup(c => c.PostWithResponseCodeAsMultipart<ValidateRolloverExtensionCommandResponse>(
                    It.IsAny<ValidateRolloverExtensionApiRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            _apiClientMock.Verify(c =>
                c.PostWithResponseCodeAsMultipart<ValidateRolloverExtensionCommandResponse>(
                    It.IsAny<ValidateRolloverExtensionApiRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.IsValid, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.ValidationSuccessSummary, Is.Not.Null);

            var summary = result.Value.ValidationSuccessSummary;

            Assert.That(summary.TotalCandidatesCount, Is.EqualTo(25));
            Assert.That(summary.CandidatesExtendedInUploadCount, Is.EqualTo(4));
            Assert.That(summary.TotalCandidatesToBeExcludedCount, Is.EqualTo(14));
            Assert.That(summary.TotalCandidatesToBeExtendedCount, Is.EqualTo(8));
            Assert.That(summary.TotalCandidatesToBeReviewedCount, Is.EqualTo(3));
        }


        [Test]
        public async Task Handle_WhenApiClientThrows_ShouldReturnFailureResponse()
        {
            // Arrange
            var command = _fixture.Create<ValidateRolloverExtensionCommand>();

            _apiClientMock
                .Setup(x => x.PostWithResponseCodeAsMultipart<ValidateRolloverExtensionCommandResponse>(
                    It.IsAny<ValidateRolloverExtensionApiRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("API failure"));

            _apiClientMock.Verify(x =>
                x.PostWithResponseCodeAsMultipart<ValidateRolloverExtensionCommandResponse>(
                    It.IsAny<ValidateRolloverExtensionApiRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_ShouldSendCorrectApiRequest()
        {
            // Arrange
            var command = _fixture.Create<ValidateRolloverExtensionCommand>();

            ValidateRolloverExtensionApiRequest? capturedRequest = null;

            _apiClientMock
                .Setup(x => x.PostWithResponseCodeAsMultipart<ValidateRolloverExtensionCommandResponse>(
                    It.IsAny<IPostMultipartFormDataApiRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IPostMultipartFormDataApiRequest, CancellationToken>((req, _) =>
                {
                    capturedRequest = req as ValidateRolloverExtensionApiRequest;
                })
                .ReturnsAsync(new ApiResponse<ValidateRolloverExtensionCommandResponse>(
                    new ValidateRolloverExtensionCommandResponse
                    {
                        IsValid = true
                    },
                    HttpStatusCode.OK,
                    string.Empty));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Data, Is.EqualTo(command));
            Assert.That(
                capturedRequest.FormData,
                Does.Contain(new KeyValuePair<string, string>(
                    "RolloverCandidates[0].Qan",
                    command.RolloverCandidates[0].Qan)));
        }

    }
}
