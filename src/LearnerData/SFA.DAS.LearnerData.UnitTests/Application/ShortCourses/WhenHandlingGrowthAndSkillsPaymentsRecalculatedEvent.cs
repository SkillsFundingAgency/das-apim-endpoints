using AutoFixture;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;

namespace SFA.DAS.LearnerData.UnitTests.Application.ShortCourses;

[TestFixture]
public class WhenHandlingGrowthAndSkillsPaymentsRecalculatedEvent
{
    private Fixture _fixture = new Fixture();
    private Mock<ILogger<GrowthAndSkillsPaymentsRecalculatedEventHandler>> _logger;
    private Mock<IMessageHandlerContext> _context;
    private GrowthAndSkillsPaymentsRecalculatedEventHandler _handler;
    private PaymentsConfiguration _paymentsConfiguration;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<GrowthAndSkillsPaymentsRecalculatedEventHandler>>();
        _context = new Mock<IMessageHandlerContext>();
        _paymentsConfiguration = new PaymentsConfiguration { PaymentsEndpoint = "test-payments-endpoint" };

        _handler = new GrowthAndSkillsPaymentsRecalculatedEventHandler(_logger.Object, _paymentsConfiguration);
    }

    [Test]
    public async Task Then_CalculateGrowthAndSkillsPayments_Command_Is_Sent()
    {
        // Arrange
        var command = _fixture.Create<CalculateGrowthAndSkillsPayments>();
        var message = new GrowthAndSkillsPaymentsRecalculatedEvent { Command = command };

        // Act
        await _handler.Handle(message, _context.Object);

        // Assert
        _context.Verify(x => x.Send(
            It.Is<object>(o => o == command),
            It.IsAny<SendOptions>()),
            Times.Once);
    }

    [Test]
    public async Task Then_Command_Is_Not_Sent_More_Than_Once()
    {
        // Arrange
        var message = new GrowthAndSkillsPaymentsRecalculatedEvent
        {
            Command = _fixture.Create<CalculateGrowthAndSkillsPayments>()
        };

        // Act
        await _handler.Handle(message, _context.Object);

        // Assert
        _context.Verify(x => x.Send(It.IsAny<object>(), It.IsAny<SendOptions>()), Times.Once);
    }
}
