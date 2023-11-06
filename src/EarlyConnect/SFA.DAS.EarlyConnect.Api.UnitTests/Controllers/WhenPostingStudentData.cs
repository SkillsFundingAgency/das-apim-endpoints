using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.StudentData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingStudentData
    {
        private StudentDataController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<StudentDataController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<StudentDataController>>();
            _controller = new StudentDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Post_ValidRequest_ReturnsOkResult()
        {
            var request = new CreateStudentDataPostRequest
            {
                ListOfStudentData = new List<StudentRequestModel>()
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.Post(request);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateStudentDataPostRequest
            {
                ListOfStudentData = new List<StudentRequestModel>()
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.Post(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

