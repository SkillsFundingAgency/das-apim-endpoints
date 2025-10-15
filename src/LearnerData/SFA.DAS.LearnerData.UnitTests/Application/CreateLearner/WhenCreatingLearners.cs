using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.UnitTests.Application.CreateLearner;

public class WhenCreatingLearners
{
    private Fixture _fixture;

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
    private Mock<IMessageSession> _mockMessageSession;
    private Mock<ILogger<CreateLearnerCommandHandler>> _mockLogger;
    private CreateLearnerCommandHandler _sut;


    public WhenCreatingLearners()
    {
        _fixture = new Fixture();
    }
#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

    [SetUp]
    public void Setup()
    {
        _mockMessageSession = new Mock<IMessageSession>();
        _mockLogger = new Mock<ILogger<CreateLearnerCommandHandler>>();
        _sut = new CreateLearnerCommandHandler(
            _mockLogger.Object,
            _mockMessageSession.Object);
    }


    [Test]
    public async Task Then_call_is_successful()
    {
        // Arrange
        var command = GetProcessLearnersCommand();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _mockMessageSession.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()),
            Times.Once());
    }

    [Test]
    public async Task Then_call_is_successful_and_request_fields_maps_to_event()
    {
        // Arrange
        var request = _fixture.Create<CreateLearnerRequest>();
        var command = GetProcessLearnersCommand(request);

        var @event = new LearnerDataEvent();
        _mockMessageSession.Setup(x => x.Publish(It.IsAny<LearnerDataEvent>(), It.IsAny<PublishOptions>()))
            .Callback((object p, PublishOptions o) =>
            {
                @event = (LearnerDataEvent)p;
            });

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        @event.Should().BeEquivalentTo(new
        {
            ULN = long.Parse(request.Learner.Uln),
            UKPRN = command.Ukprn,
            FirstName = request.Learner.Firstname,
            LastName = request.Learner.Lastname,
            Email = request.Learner.Email,
            DoB = request.Learner.Dob!.Value,
            StartDate = request.Delivery.OnProgramme.StartDate!.Value,
            PlannedEndDate = request.Delivery.OnProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = request.Delivery.OnProgramme.PercentageOfTrainingLeft,
            EpaoPrice = request.Delivery.OnProgramme.Costs.Single().EpaoPrice,
            TrainingPrice = request.Delivery.OnProgramme.Costs.Single().TrainingPrice,
            AgreementId = request.Delivery.OnProgramme.AgreementId,
            IsFlexiJob = request.Delivery.OnProgramme.IsFlexiJob!.Value,
            StandardCode = request.Delivery.OnProgramme.StandardCode!.Value,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = request.ConsumerReference
        });
    }

    private CreateLearnerCommand GetProcessLearnersCommand(CreateLearnerRequest? createLearnerRequest = null)
    {
        var command = _fixture.Create<CreateLearnerCommand>();
        if (createLearnerRequest != null)
        {
            command.Request = createLearnerRequest;
        }

        command.Request.Learner.Uln = $"{_fixture.Create<ulong>()}";
        command.Request.Delivery.OnProgramme.Costs = new List<CostDetails> { _fixture.Create<CostDetails>() };

        return command;
    }
}