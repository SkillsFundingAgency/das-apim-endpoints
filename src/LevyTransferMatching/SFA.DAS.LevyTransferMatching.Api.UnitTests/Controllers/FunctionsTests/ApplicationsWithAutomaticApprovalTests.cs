using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class ApplicationsWithAutomaticApprovalTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Action_Calls_Handler()
        {
            var result = await _controller.ApplicationsWithAutomaticApproval();

            _mediator.Verify(x =>
                x.Send(It.IsAny<ApplicationsWithAutomaticApprovalQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ApplicationsWithAutomaticApproval_WhenQuerySucceeds_ReturnsOkResultWithData()
        {
            // Arrange
            var expectedResult = new ApplicationsWithAutomaticApprovalQueryResult();
            _mediator.Setup(x => x.Send(It.IsAny<ApplicationsWithAutomaticApprovalQuery>(), default))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ApplicationsWithAutomaticApproval();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedResult, okResult.Value);
        }

       
    }
}
