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
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Sectors;
using SFA.DAS.Campaign.Application.Queries.Standards;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenGettingStandardsBySector
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_By_Sector_From_Mediator(
            string sector,
            GetStandardsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsQuery>(c=>c.Sector.Equals(sector)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandardsBySector(sector) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardsResponse;
            Assert.IsNotNull(model);
            model.Standards.Should().BeEquivalentTo(mediatorResult.Standards, options => options);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetSectorsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetStandardsBySector("") as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}