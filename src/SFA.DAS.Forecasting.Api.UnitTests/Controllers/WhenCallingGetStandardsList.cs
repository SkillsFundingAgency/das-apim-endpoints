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
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetStandardCoursesList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers
{
    public class WhenCallingGetStandardsList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Mediator(
            GetStandardCoursesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandardsList() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardsListResponse;
            Assert.IsNotNull(model);
            model.Standards.Should().BeEquivalentTo(mediatorResult.Standards,
                o => o
                    .Excluding(s => s.ApprenticeshipFunding)
                    .Excluding(s => s.MaxFunding)
                    .Excluding(s => s.TypicalDuration)
                    .Excluding(s => s.StandardDates)
                );
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetFrameworksList() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}