using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Application
{
    [TestFixture]
    public class BulkApplicationActionCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private BulkApplicationActionCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _handler = new BulkApplicationActionCommandHandler(_mediatorMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenAllShareWithOfqualCommandsSucceed()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.ShareWithOfqual)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse>
                {
                    Success = true,
                    Value = new EmptyResponse()
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(0));
                Assert.That(result.Value.Errors, Is.Empty);
            });

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()),
                Times.Exactly(applicationReviewIds.Count));
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenAllShareWithSkillsEnglandCommandsSucceed()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.ShareWithSkillsEngland)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse>
                {
                    Success = true,
                    Value = new EmptyResponse()
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(0));
                Assert.That(result.Value.Errors, Is.Empty);
            });

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()),
                Times.Exactly(applicationReviewIds.Count));
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenAllUnlockCommandsSucceed()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.Unlock)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
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
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(0));
                Assert.That(result.Value.Errors, Is.Empty);
            });

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()),
                Times.Exactly(applicationReviewIds.Count));
        }

        [Test]
        public async Task Handle_RecordsUpdateFailedErrors_WhenSomeShareCommandsFail()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.ShareWithOfqual)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            _mediatorMock
                .SetupSequence(x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse> { Success = true, Value = new EmptyResponse() })
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse> { Success = false, Value = new EmptyResponse() })
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse> { Success = true, Value = new EmptyResponse() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(2));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(1));
                Assert.That(result.Value.Errors.Count, Is.EqualTo(1));
                Assert.That(result.Value.Errors.First()?.ApplicationReviewId, Is.EqualTo(applicationReviewIds[1]));
                Assert.That(result.Value.Errors.First()?.ErrorType, Is.EqualTo(BulkApplicationActionErrorType.UpdateFailed));
            });
        }

        [Test]
        public async Task Handle_RecordsUpdateFailedErrors_WhenSomeUnlockCommandsFail()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.Unlock)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            _mediatorMock
                .SetupSequence(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse>
                {
                    Success = true,
                    Value = new CreateApplicationMessageCommandResponse()
                })
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse>
                {
                    Success = false,
                    Value = new CreateApplicationMessageCommandResponse()
                })
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
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(2));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(1));
                Assert.That(result.Value.Errors.Count, Is.EqualTo(1));
                Assert.That(result.Value.Errors.First()?.ApplicationReviewId, Is.EqualTo(applicationReviewIds[1]));
                Assert.That(result.Value.Errors.First()?.ErrorType, Is.EqualTo(BulkApplicationActionErrorType.UpdateFailed));
            });
        }

        [Test]
        public async Task Handle_CallsMediatorWithCorrectUnlockMessageType()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.Unlock)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            CreateApplicationMessageCommand? captured = null;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<BaseMediatrResponse<CreateApplicationMessageCommandResponse>>, CancellationToken>((cmd, _) =>
                    captured = (CreateApplicationMessageCommand)cmd)
                .ReturnsAsync(new BaseMediatrResponse<CreateApplicationMessageCommandResponse>
                {
                    Success = true,
                    Value = new CreateApplicationMessageCommandResponse()
                });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(captured!.ApplicationId, Is.EqualTo(applicationReviewIds[0]));
                Assert.That(captured.MessageType, Is.EqualTo(ApplicationMessageType.UnlockApplication.ToString()));
                Assert.That(captured.MessageText, Is.EqualTo($"Bulk Action {ApplicationMessageType.UnlockApplication}"));
                Assert.That(captured.SentByEmail, Is.EqualTo(command.SentByEmail));
                Assert.That(captured.SentByName, Is.EqualTo(command.SentByName));
                Assert.That(captured.UserType, Is.EqualTo(command.UserType));
            });
        }

        [Test]
        public async Task Handle_CallsMediatorWithCorrectShareWithOfqualCommandData()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.ShareWithOfqual)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            UpdateApplicationReviewSharingCommand? captured = null;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<BaseMediatrResponse<EmptyResponse>>, CancellationToken>((cmd, _) =>
                    captured = (UpdateApplicationReviewSharingCommand)cmd)
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse>
                {
                    Success = true,
                    Value = new EmptyResponse()
                });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(captured!.ApplicationReviewId, Is.EqualTo(applicationReviewIds[0]));
                Assert.That(captured.ApplicationReviewUserType, Is.EqualTo("Ofqual"));
                Assert.That(captured.ShareApplication, Is.True);
                Assert.That(captured.SentByEmail, Is.EqualTo(command.SentByEmail));
                Assert.That(captured.SentByName, Is.EqualTo(command.SentByName));
                Assert.That(captured.UserType, Is.EqualTo(command.UserType));
            });
        }

        [Test]
        public async Task Handle_CallsMediatorWithCorrectShareWithSkillsEnglandCommandData()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid() };

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, BulkApplicationActionType.ShareWithSkillsEngland)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            UpdateApplicationReviewSharingCommand? captured = null;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<BaseMediatrResponse<EmptyResponse>>, CancellationToken>((cmd, _) =>
                    captured = (UpdateApplicationReviewSharingCommand)cmd)
                .ReturnsAsync(new BaseMediatrResponse<EmptyResponse>
                {
                    Success = true,
                    Value = new EmptyResponse()
                });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(captured!.ApplicationReviewId, Is.EqualTo(applicationReviewIds[0]));
                Assert.That(captured.ApplicationReviewUserType, Is.EqualTo("SkillsEngland"));
                Assert.That(captured.ShareApplication, Is.True);
                Assert.That(captured.SentByEmail, Is.EqualTo(command.SentByEmail));
                Assert.That(captured.SentByName, Is.EqualTo(command.SentByName));
                Assert.That(captured.UserType, Is.EqualTo(command.UserType));
            });
        }

        [Test]
        public async Task Handle_AddsInvalidActionError_ForEachApplicationId_WhenActionTypeIsInvalid()
        {
            // Arrange
            var applicationReviewIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            var invalidActionType = (BulkApplicationActionType)999;

            var command = _fixture.Build<BulkApplicationActionCommand>()
                .With(x => x.ActionType, invalidActionType)
                .With(x => x.ApplicationReviewIds, applicationReviewIds)
                .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(0));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.Errors.Count, Is.EqualTo(applicationReviewIds.Count));
                Assert.That(result.Value.Errors.All(x => x.ErrorType == BulkApplicationActionErrorType.InvalidAction), Is.True);
                Assert.That(result.Value.Errors.Select(x => x.ApplicationReviewId), Is.EquivalentTo(applicationReviewIds));
            });

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<CreateApplicationMessageCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _mediatorMock.Verify(
                x => x.Send(It.IsAny<UpdateApplicationReviewSharingCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}