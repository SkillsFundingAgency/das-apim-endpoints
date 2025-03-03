using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    [TestFixture]
    public class WhenHandlingAcknowledgeProviderResponses
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _apiClientMock;
        private AcknowledgeProviderResponsesCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _sut = new AcknowledgeProviderResponsesCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCallApiClientWithCorrectRequest()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                AcknowledgedBy = Guid.NewGuid()
            };

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(client => client.Put(
                It.Is<PutAcknowledgeProviderResponsesRequest>(req =>
                    req.EmployerRequestId == command.EmployerRequestId &&
                    req.Data.AcknowledgedBy == command.AcknowledgedBy
                )), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenApiClientThrowsException()
        {
            // Arrange
            var command = new AcknowledgeProviderResponsesCommand
            {
                EmployerRequestId = Guid.NewGuid(),
                AcknowledgedBy = Guid.NewGuid()
            };

            _apiClientMock.Setup(client => client.Put(It.IsAny<PutAcknowledgeProviderResponsesRequest>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _sut.Handle(command, CancellationToken.None));
            _apiClientMock.Verify(client => client.Put(It.IsAny<PutAcknowledgeProviderResponsesRequest>()), Times.Once);
        }
    }
}
