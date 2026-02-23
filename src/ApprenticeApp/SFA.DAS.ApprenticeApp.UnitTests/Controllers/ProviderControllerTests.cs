using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests.Controllers
{
    public class ProviderControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private ProviderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProviderController(_mediatorMock.Object);
        }

        [Test]
        public async Task GetRegisteredProviders_ShouldReturn_ProvidersFromMediator()
        {
            // Arrange
            var providers = new List<RoatpProvider>
            {
                new RoatpProvider { Ukprn = 12345678, Name = "Test Provider" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoatpProvidersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetRoatpProvidersQueryResult { Providers = providers });

            // Act
            var result = await _controller.GetRegisteredProviders();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(providers);

            _mediatorMock.Verify(m =>
                m.Send(It.IsAny<GetRoatpProvidersQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GetRegisteredProviders_ShouldReturn_EmptyList_WhenNoProviders()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetRoatpProvidersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetRoatpProvidersQueryResult { });

            // Act
            var result = await _controller.GetRegisteredProviders();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(new List<RoatpProvider>());
        }
    }
}