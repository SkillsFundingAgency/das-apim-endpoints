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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.Sectors;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Sectors
{
    public class WhenGettingSectors
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Sectors_From_Mediator(
            GetSectorsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]SectorsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetSectorsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetSectors() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetSectorsResponse;
            Assert.IsNotNull(model);

            foreach (var sector in model.Sectors)
            {
                mediatorResult.Sectors.Should().Contain(c => c.Name.Equals(sector.Route));
                mediatorResult.Sectors.Should().Contain(c => c.Id.Equals(sector.Id));
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]SectorsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetSectorsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetSectors() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}