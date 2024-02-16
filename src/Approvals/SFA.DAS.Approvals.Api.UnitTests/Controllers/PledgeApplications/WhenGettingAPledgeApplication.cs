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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.PledgeApplications
{
    public class WhenGettingAPledgeApplication
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_PledgeApplication_From_Mediator(
            int pledgeApplicationId,
            GetPledgeApplicationResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeApplicationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetPledgeApplicationQuery>(x=>x.PledgeApplicationId == pledgeApplicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(pledgeApplicationId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetPledgeApplicationResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo((GetPledgeApplicationResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Apprentice_Is_Returned_From_Mediator(
            int pledgeApplicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeApplicationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetPledgeApplicationQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var controllerResult = await controller.Get(pledgeApplicationId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int pledgeApplicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeApplicationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetPledgeApplicationQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(pledgeApplicationId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}