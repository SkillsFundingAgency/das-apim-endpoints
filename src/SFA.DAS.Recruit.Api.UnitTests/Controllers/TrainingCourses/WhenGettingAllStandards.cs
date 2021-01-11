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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetStandards;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenGettingAllStandards
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Mediator(
            GetStandardsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandards() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardsListResponse;
            Assert.IsNotNull(model);
            model.Standards.Should().BeEquivalentTo(mediatorResult.Standards, options=>options
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(c=>c.StandardDates)
                .Excluding(c=>c.TypicalDuration)
            );
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

            var controllerResult = await controller.GetStandards() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}