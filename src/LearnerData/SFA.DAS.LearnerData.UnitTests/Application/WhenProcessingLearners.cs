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
    public async Task Then_call_is_successful_and_event_maps_to_request(
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

        await sut.Handle(new ProcessLearnersCommand {Learners = [request]}, CancellationToken.None);

        @event.Should().BeEquivalentTo(request);
    }
}