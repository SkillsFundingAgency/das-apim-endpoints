using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Models;
using MediatR;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Application
{
    [TestFixture]
    public class BulkApplicationMessageCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private BulkApplicationMessageCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _handler = new BulkApplicationMessageCommandHandler(_mediatorMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenAllCommandsSucceed()
        {
            // Arrange
            var command = _fixture.Build<BulkApplicationMessageCommand>()
                .With(x => x.ShareWithSkillsEngland, true)
                .With(x => x.ShareWithOfqual, false)
                .With(x => x.Unlock, false)
                .Create();

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse>
                {
                    Success = true,
                    Value = new CreateApplicationMessageCommandResponse()
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(command.ApplicationIds.Count));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(0));
            });

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()),
                Times.Exactly(command.ApplicationIds.Count));
        }

        [Test]
        public async Task Handle_RecordsErrors_WhenSomeCommandsFail()
        {
            // Arrange
            var command = _fixture.Build<BulkApplicationMessageCommand>()
                .With(x => x.ShareWithOfqual, true)
                .With(x => x.ShareWithSkillsEngland, false)
                .With(x => x.Unlock, false)
                .Create();

            var ids = command.ApplicationIds.ToList();

            _mediatorMock
                .SetupSequence(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse> { Success = true })
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse> { Success = false })
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse> { Success = true });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(2));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(1));
                Assert.That(result.Value.Errors.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task Handle_CallsMediatorWithCorrectMessageType()
        {
            // Arrange
            var command = _fixture.Build<BulkApplicationMessageCommand>()
                .With(x => x.Unlock, true)
                .With(x => x.ShareWithSkillsEngland, false)
                .With(x => x.ShareWithOfqual, false)
                .Create();

            CreateApplicationMessageCommand? captured = null;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((cmd, _) => captured = (CreateApplicationMessageCommand)cmd)
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse> { Success = true });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.MessageType, Is.EqualTo(ApplicationMessageType.UnlockApplication.ToString()));
        }

        [Test]
        public async Task Handle_CallsMediatorWithCorrectCommandData()
        {
            // Arrange
            var command = _fixture.Build<BulkApplicationMessageCommand>()
                .With(x => x.ShareWithSkillsEngland, true)
                .With(x => x.ShareWithOfqual, false)
                .With(x => x.Unlock, false)
                .Create();

            CreateApplicationMessageCommand? captured = null;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((cmd, _) => captured = (CreateApplicationMessageCommand)cmd)
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse> { Success = true });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(captured, Is.Not.Null);
                Assert.That(captured!.SentByEmail, Is.EqualTo(command.SentByEmail));
                Assert.That(captured.SentByName, Is.EqualTo(command.SentByName));
                Assert.That(captured.UserType, Is.EqualTo(command.UserType));
                Assert.That(captured.MessageText, Does.Contain("Bulk Action"));
            });
        }

        [Test]
        public void Handle_Throws_WhenNoBulkActionSelected()
        {
            // Arrange
            var command = _fixture.Build<BulkApplicationMessageCommand>()
                .With(x => x.ShareWithSkillsEngland, false)
                .With(x => x.ShareWithOfqual, false)
                .With(x => x.Unlock, false)
                .Create();

            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
