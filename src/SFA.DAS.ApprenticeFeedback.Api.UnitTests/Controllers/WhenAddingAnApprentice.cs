using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.ApiRequests;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenAddingAnApprentice
    {
        private Mock<IMediator> _mockMediator;

        private ApprenticeController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();

            _controller = new ApprenticeController(_mockMediator.Object, Mock.Of<ILogger<ApprenticeController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnCreated(
            CreateApprentice request)
        {
            var result = await _controller.AddApprentice(request) as CreatedResult;

            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task And_CommandThrowsException_Then_ReturnBadRequest(CreateApprentice request)
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateApprenticeCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var result = await _controller.AddApprentice(request) as StatusCodeResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
