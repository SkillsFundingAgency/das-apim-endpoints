using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticePortal.Api.Controllers;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.ApprenticePortal.Application.Homepage.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.Controllers
{
    public class ApprenticeHomepageControllerTests
    {
        [Test, MoqAutoData]
        public async Task TestGetHomepageApprentice(
            GetApprenticeHomepageQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticeHomepageController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(
                It.IsAny<GetApprenticeHomepageQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var response = await controller.GetHomepageApprentice(new Guid()) as ObjectResult;

            // Assert
            Assert.IsNotNull(response);
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = response.Value as ApprenticeHomepage;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(result.apprenticeHomepage);
        }
    }
}
