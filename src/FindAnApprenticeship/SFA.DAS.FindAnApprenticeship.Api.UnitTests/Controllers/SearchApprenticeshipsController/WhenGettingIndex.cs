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
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController
{
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Apprenticeships_From_Mediator(
            SearchApprenticeshipsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SearchApprenticeshipsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Index() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as SearchApprenticeshipsApiResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((SearchApprenticeshipsApiResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SearchApprenticeshipsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.Index() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}