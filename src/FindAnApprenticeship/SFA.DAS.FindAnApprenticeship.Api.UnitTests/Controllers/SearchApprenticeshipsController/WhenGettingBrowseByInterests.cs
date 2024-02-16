using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController
{
    public class WhenGettingBrowseByInterests
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Routes_From_Mediator(
            BrowseByInterestsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.IsAny<BrowseByInterestsQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(mediatorResult);

            var controllerResult = await controller.BrowseByInterests() as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as BrowseByInterestsApiResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo((BrowseByInterestsApiResponse) mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<BrowseByInterestsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.BrowseByInterests() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}