using SFA.DAS.LearnerData.Application;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.UnitTests.Application;

public class WhenProcessingLearners
{
    [Test, MoqAutoData]
    public async Task Then_call_is_successful(
        ProcessLearnersCommand command,
        [Frozen] Mock<ILogger<ProcessLearnersCommandHandler>> mockLogger,
        [Frozen] Mock<IMessageSession> mockMessageSession,
        ProcessLearnersCommandHandler sut)
    {
        await sut.Handle(command, CancellationToken.None);
        mockMessageSession.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()),
            Times.Exactly(command.Learners.Count()));
    }

    [Test, MoqAutoData]
    public async Task Then_call_is_successful_and_request_fields_maps_to_event(
        Guid correlationId,
        DateTime recievedOn,
        int academicYear,
        LearnerDataRequest request,
        [Frozen] Mock<ILogger<ProcessLearnersCommandHandler>> mockLogger,
        [Frozen] Mock<IMessageSession> mockMessageSession,
        ProcessLearnersCommandHandler sut)
    {
        var @event = new LearnerDataEvent();
        mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        await sut.Handle(
            new ProcessLearnersCommand
            {
                CorrelationId = correlationId, ReceivedOn = recievedOn, AcademicYear = academicYear,
                Learners = [request]
            }, CancellationToken.None);

        @event.Should().BeEquivalentTo(request);
    }

    [Test, MoqAutoData]
    public async Task Then_call_is_successful_and_new_fields_maps_to_event(
        Guid correlationId,
        DateTime recievedOn,
        int academicYear,
        LearnerDataRequest request,
        [Frozen] Mock<ILogger<ProcessLearnersCommandHandler>> mockLogger,
        [Frozen] Mock<IMessageSession> mockMessageSession,
        ProcessLearnersCommandHandler sut)
    {
        var @event = new LearnerDataEvent();
        mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        await sut.Handle(
            new ProcessLearnersCommand
            {
                CorrelationId = correlationId,
                ReceivedOn = recievedOn,
                AcademicYear = academicYear,
                Learners = [request]
            }, CancellationToken.None);

        @event.CorrelationId.Should().Be(correlationId);
        @event.ReceivedOn.Should().Be(recievedOn);
        @event.AcademicYear.Should().Be(academicYear);
    }

}