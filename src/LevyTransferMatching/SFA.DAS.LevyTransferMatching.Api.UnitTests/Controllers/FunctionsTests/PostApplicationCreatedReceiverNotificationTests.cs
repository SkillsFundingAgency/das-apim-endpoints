using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;

[TestFixture]
public class PostApplicationCreatedReceiverNotificationTests
{
    private FunctionsController _controller;
    private Mock<IMediator> _mediator;
    private ApplicationCreatedReceiverNotificationRequest _request;
    private readonly Fixture _fixture = new ();

    [SetUp]
    public void Setup()
    {
        _request = _fixture.Create<ApplicationCreatedReceiverNotificationRequest>();

        _mediator = new Mock<IMediator>();

        _controller = new FunctionsController(_mediator.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunctionsController>>());
    }

    [Test]
    public async Task Post_ApplicationApprovedReceiverNotificationRequest()
    {
        await _controller.ApplicationCreatedReceiverNotification(_request);

        _mediator.Verify(x =>
            x.Send(It.Is<ApplicationCreatedEmailCommand>(c =>
                c.PledgeId == _request.PledgeId
                && c.ApplicationId == _request.ApplicationId
                && c.EncodedApplicationId == _request.EncodedApplicationId
                && c.ReceiverId == _request.ReceiverId
                && c.UnsubscribeUrl == _request.UnsubscribeUrl),
                It.IsAny<CancellationToken>()));
    }
}
