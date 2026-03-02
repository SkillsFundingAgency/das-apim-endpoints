using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Qualifications;
using SFA.DAS.AODP.Application.Commands.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Qualifications
{
    [TestFixture]
    public class BulkUpdateQualificationStatusCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private BulkUpdateQualificationStatusCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new BulkUpdateQualificationStatusCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();
            var apiResponse = new BulkUpdateQualificationStatusCommandResponse
            {
                RequestedCount = 2,
                UpdatedCount = 2,
                ErrorCount = 0
            };

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.IsAny<BulkUpdateQualificationStatusApiRequest>()))
                .ReturnsAsync(new ApiResponse<BulkUpdateQualificationStatusCommandResponse>(
                    apiResponse, System.Net.HttpStatusCode.OK, ""));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(apiResponse.RequestedCount));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(apiResponse.UpdatedCount));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(apiResponse.ErrorCount));
            });

            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.IsAny<BulkUpdateQualificationStatusApiRequest>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsError_WhenApiCallThrows()
        {
            // Arrange
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();
            var exceptionMessage = "API failed";

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.IsAny<BulkUpdateQualificationStatusApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
            });

            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.IsAny<BulkUpdateQualificationStatusApiRequest>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_CallsApiClientWithCorrectRequest()
        {
            // Arrange
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.IsAny<BulkUpdateQualificationStatusApiRequest>()))
                .ReturnsAsync(new ApiResponse<BulkUpdateQualificationStatusCommandResponse>(
                    new BulkUpdateQualificationStatusCommandResponse(), System.Net.HttpStatusCode.OK, ""));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkUpdateQualificationStatusCommandResponse>(
                    It.Is<BulkUpdateQualificationStatusApiRequest>(req =>
                        ((BulkUpdateQualificationStatusCommand)req.Data).ProcessStatusId == command.ProcessStatusId &&
                        ((BulkUpdateQualificationStatusCommand)req.Data).Comment == command.Comment &&
                        ((BulkUpdateQualificationStatusCommand)req.Data).UserDisplayName == command.UserDisplayName &&
                        ((BulkUpdateQualificationStatusCommand)req.Data).QualificationIds.SequenceEqual(command.QualificationIds)
                    )),
                Times.Once);
        }
    }
}
