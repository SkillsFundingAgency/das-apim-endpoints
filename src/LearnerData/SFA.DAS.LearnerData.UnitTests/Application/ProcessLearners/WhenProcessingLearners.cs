using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.ProcessLearners;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.UnitTests.Application.ProcessLearners;

public class WhenProcessingLearners
{
    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<IMessageSession> _mockMessageSession;
    private Mock<ILogger<ProcessLearnersCommandHandler>> _mockLogger;
    private ProcessLearnersCommandHandler _sut;


    public WhenProcessingLearners()
    {
        _fixture = new Fixture();
    }
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _mockMessageSession = new Mock<IMessageSession>();
        _mockLogger = new Mock<ILogger<ProcessLearnersCommandHandler>>();
        _sut = new ProcessLearnersCommandHandler(
            _mockLogger.Object,
            _mockMessageSession.Object);
    }


    [Test]
    public async Task Then_call_is_successful()
    {
        // Arrange
        var command = _fixture.Create<ProcessLearnersCommand>();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockMessageSession.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()),
            Times.Exactly(command.Learners.Count()));
    }

    [Test]
    public async Task Then_call_is_successful_and_request_fields_maps_to_event()
    {
        // Arrange
        var correlationId = Guid.NewGuid();
        var receivedOn = DateTime.UtcNow;
        var academicYear = 2223;
        var request = _fixture.Create<LearnerDataRequest>();
        var @event = new LearnerDataEvent();
        _mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        // Act
        await _sut.Handle(
            new ProcessLearnersCommand
            {
                CorrelationId = correlationId,
                ReceivedOn = receivedOn,
                AcademicYear = academicYear,
                Learners = [request]
            }, CancellationToken.None);

        // Assert
        @event.Should().BeEquivalentTo(new
        {
            request.ULN,
            request.UKPRN,
            request.FirstName,
            request.LastName,
            Email = request.LearnerEmail,
            DoB = request.DateOfBirth,
            request.StartDate,
            request.PlannedEndDate,
            request.PercentageLearningToBeDelivered,
            request.EpaoPrice,
            request.TrainingPrice,
            request.AgreementId,
            request.IsFlexiJob,
            PlannedOTJTrainingHours = request.PlannedOTJTrainingHours ?? 0,
            request.StandardCode,
            CorrelationId = correlationId,
            ReceivedDate = receivedOn,
            AcademicYear = academicYear,
            request.ConsumerReference
        });
    }

    [Test]
    public async Task Then_call_is_successful_and_new_fields_maps_to_event()
    {
        // Arrange
        var correlationId = Guid.NewGuid();
        var receivedOn = DateTime.UtcNow;
        var academicYear = 2223;
        var request = _fixture.Create<LearnerDataRequest>();

        var @event = new LearnerDataEvent();
        _mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        // Act
        await _sut.Handle(
            new ProcessLearnersCommand
            {
                CorrelationId = correlationId,
                ReceivedOn = receivedOn,
                AcademicYear = academicYear,
                Learners = [request]
            }, CancellationToken.None);

        // Assert
        @event.CorrelationId.Should().Be(correlationId);
        @event.ReceivedDate.Should().Be(receivedOn);
        @event.AcademicYear.Should().Be(academicYear);
    }

}