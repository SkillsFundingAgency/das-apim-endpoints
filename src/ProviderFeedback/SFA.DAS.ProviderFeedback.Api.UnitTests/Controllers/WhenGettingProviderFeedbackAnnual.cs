using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Api.Controllers;
using SFA.DAS.ProviderFeedback.Api.Models;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingProviderFeedbackAnnual
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<ProviderFeedbackController>> _loggerMock;

        private ProviderFeedbackController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ProviderFeedbackController>>();
            _controller = new ProviderFeedbackController(_loggerMock.Object, _mediatorMock.Object);
        }

        [Test, MoqAutoData]
        public async Task GetProviderFeedback_ValidRequest_ReturnsOk(
            GetProviderFeedbackAnnualResult mediatorResult,
            int ProviderId
        )
        {
            _mediatorMock
                          .Setup(m => m.Send(It.IsAny<GetProviderFeedbackAnnualQuery>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mediatorResult);

            var result = await _controller.GetProviderFeedbackAnnual(ProviderId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<GetProviderFeedbackAnnualResponse>());
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test, MoqAutoData]
        public async Task GetProviderFeedback_ValidRequest_ReturnsNotFound(
            GetProviderFeedbackAnnualResult mediatorResult,
            int ProviderId
        )
        {
            mediatorResult.ProviderStandard = null;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProviderFeedbackAnnualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await _controller.GetProviderFeedbackAnnual(ProviderId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
