using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Api.Controllers;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetStandardOptions;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.Api.UnitTests.Controllers
{
    public class WhenGettingStandardOptions
    {

        [Test, MoqAutoData]
        public async Task Then_gets_Standard_and_options_from_mediator(GetStandardOptionsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator.Setup(mediator => mediator.Send(It.IsAny<GetStandardOptionsQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandardOptionsList() as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetStandardOptionsResponse;

            var response = mediatorResult.Standards.Select(s => new GetStandardOptionsItem
            {
                StandardUId = s.StandardUId,
                LarsCode = s.LarsCode,
                IfateReferenceNumber = s.IfateReferenceNumber,
                Options = s.Options
            });

            model.Standards.Should().BeEquivalentTo(response);
        }
    }
}
