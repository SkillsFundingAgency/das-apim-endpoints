using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Controllers;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Controllers.Epaos
{
    public class WhenGettingEpaos
    {
        [Test, MoqAutoData, Ignore("for now")]
        public async Task Then_Gets_Training_Courses_From_Mediator(
            GetEpaosResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEpaosQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList() as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEpaosApiModel;
            model.Epaos.Should().BeEquivalentTo(mediatorResult.Epaos);
        }
    }
}