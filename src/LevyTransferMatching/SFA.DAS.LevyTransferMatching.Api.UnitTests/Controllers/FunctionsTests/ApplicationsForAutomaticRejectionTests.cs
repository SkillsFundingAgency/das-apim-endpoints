﻿using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class ApplicationsForAutomaticRejectionTests
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
            // Arrange
            var fixture = new Fixture();
            var expectedResult = fixture.Create<GetApplicationsForAutomaticRejectionQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApplicationsForAutomaticRejectionQuery>(), default))
                .ReturnsAsync(expectedResult);

            await _controller.ApplicationsForAutomaticRejection();

            _mediator.Verify(x =>
                x.Send(It.IsAny<GetApplicationsForAutomaticRejectionQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ApplicationsForAutomaticRejection_WhenQuerySucceeds_ReturnsOkResultWithData()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedResult = fixture.Create<GetApplicationsForAutomaticRejectionQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApplicationsForAutomaticRejectionQuery>(), default))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ApplicationsForAutomaticRejection();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            var response = okResult.Value as GetApplicationsForAutomaticRejectionResponse;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.Applications.Count(), Is.EqualTo(expectedResult.Applications.Count()));
            var i = 0;
            foreach (var item in response.Applications)
            {
                var expectedItem = expectedResult.Applications.ToArray()[i];
                Assert.That(expectedItem.Id, Is.EqualTo(item.Id));
                Assert.That(expectedItem.PledgeId, Is.EqualTo(item.PledgeId));

                i++;
            }
        }
    }
}
