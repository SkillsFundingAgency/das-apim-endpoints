﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenGettingRestartCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Demand_Is_Returned_From_Mediator(
            Guid id,
            GetRestartEmployerDemandQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.Is<GetRestartEmployerDemandQuery>(c => c.Id.Equals(id)), CancellationToken.None))
                .ReturnsAsync(queryResult);
            
            //Act
            var actual = await controller.Restart(id) as ObjectResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var actualModel = actual.Value as GetRestartCourseDemandResponse;
            Assert.IsNotNull(actualModel);
            actualModel.Id.Should().Be(queryResult.EmployerDemand.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Returned_Then_Not_Found_Response_Returned(
            Guid id,
            GetRestartEmployerDemandQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] DemandController controller)
        {
            //Arrange
            queryResult.EmployerDemand = null;
            mediator.Setup(x => x.Send(It.Is<GetRestartEmployerDemandQuery>(c => c.Id.Equals(id)), CancellationToken.None))
                .ReturnsAsync(queryResult);
            
            //Act
            var actual = await controller.Restart(id) as NotFoundResult;
            
            //Assert
            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }
    }
}