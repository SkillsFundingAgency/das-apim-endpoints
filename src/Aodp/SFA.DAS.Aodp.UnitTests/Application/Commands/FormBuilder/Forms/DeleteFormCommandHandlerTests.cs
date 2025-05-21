using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Application
{
    [TestFixture]
    public class DeleteFormCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private DeleteFormCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<DeleteFormCommandHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request()
        {
            // Arrange
            var command = _fixture.Create<DeleteFormCommand>();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _apiClientMock
                .Verify(a => a.Delete(It.Is<DeleteFormApiRequest>(r => r.FormId == command.FormId)));

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var expectedException = _fixture.Create<Exception>();
            var request = _fixture.Create<DeleteFormCommand>();
            _apiClientMock
                .Setup(a => a.Delete(It.IsAny<DeleteFormApiRequest>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(expectedException.Message));
        }
    }
}
