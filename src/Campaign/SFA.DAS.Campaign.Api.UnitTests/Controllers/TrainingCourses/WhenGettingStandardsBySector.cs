﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.Campaign.Application.Queries.Standards;
using SFA.DAS.Campaign.InnerApi.Responses;
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
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsQuery>(c => c.Sector.Equals(sector)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandards(sector) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardsResponse;
            Assert.That(model, Is.Not.Null);

            var response = mediatorResult.Standards.Select(s => new GetStandardsResponseItem 
            { 
                LarsCode = s.LarsCode,
                StandardUId = s.StandardUId,
                Level = s.Level,
                Title = s.Title
            });
            model.Standards.Should().BeEquivalentTo(response);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Returns_Empty_List_If_None_Returned_From_Standards_By_Sector_From_Mediator(
            string sector,
            GetStandardsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsQuery>(c => c.Sector.Equals(sector)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStandardsQueryResult() { Standards = new List<GetStandardsListItem>() });

            var controllerResult = await controller.GetStandards(sector) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardsResponse;
            Assert.That(model, Is.Not.Null);

            model.Standards.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetStandards("") as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}