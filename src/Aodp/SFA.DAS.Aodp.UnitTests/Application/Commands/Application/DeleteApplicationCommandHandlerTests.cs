using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Tests.Commands.Application
{
    [TestFixture]
    public class DeleteApplicationCommandHandlerTests
    {
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private const string UserType = "Qfau";
        private const string ExceptionMessage = "api exception";

        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
        private DeleteApplicationCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _handler = new DeleteApplicationCommandHandler(_apiClient.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_CallsApiClientDelete_WithUserType_AndReturnsSuccess()
        {
            var request = new DeleteApplicationCommand(ApplicationId) { UserType = UserType };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);

                _apiClient.Verify(c =>
                    c.Delete(It.Is<DeleteApplicationApiRequest>(r =>
                        r.ApplicationId == ApplicationId &&
                        r.UserType == UserType)),
                    Times.Once);
            });
        }

        [Test]
        public async Task Handle_ApiClientThrows_ReturnsErrorResponse()
        {
            _apiClient
                .Setup(c => c.Delete(It.IsAny<DeleteApplicationApiRequest>()))
                .ThrowsAsync(new InvalidOperationException(ExceptionMessage));

            var request = new DeleteApplicationCommand(ApplicationId) { UserType = UserType };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(ExceptionMessage));
            });
        }
    }
}
