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
using SFA.DAS.Campaign.Application.Queries.Adverts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.Adverts
{
    public class WhenGettingAdverts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Handled_And_Data_Returned(
            string postCode,
            string route,
            uint distance,
            GetAdvertsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] AdvertsController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetAdvertsQuery>(c=>
                    c.Distance.Equals(distance)
                    && c.Postcode.Equals(postCode)
                    && c.Route.Equals(route)
                    ), CancellationToken.None))
                .ReturnsAsync(queryResult);
            
            //Act
            var controllerResult = await controller.GetAdverts(postCode, route, distance) as ObjectResult;
            
            //Assert
            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAdvertsResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo((GetAdvertsResponse)queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_Then_Internal_Server_Error_Response_Returned(
            string postCode,
            string route,
            uint distance,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] AdvertsController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetAdvertsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());
            
            //Act
            var controllerResult = await controller.GetAdverts(postCode, route, distance) as StatusCodeResult;

            //Assert
            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}