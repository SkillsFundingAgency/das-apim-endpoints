using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCoursesList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_And_Sectors_From_Mediator(
            GetTrainingCoursesListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCoursesListQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList() as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCoursesListResponse;
            model.TrainingCourses.Should().BeEquivalentTo(mediatorResult.Courses, options=>options
                .Excluding(tc=>tc.ApprenticeshipFunding)
                .Excluding(tc=>tc.StandardDates)
            );
            model.Sectors.Should().BeEquivalentTo(mediatorResult.Sectors);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_From_Mediator_With_Keyword_And_RouteIds_If_Supplied(
            string keyword,
            List<Guid> routeIds,
            GetTrainingCoursesListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCoursesListQuery>(c=>c.Keyword.Equals(keyword) && c.RouteIds.Equals(routeIds)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(keyword, routeIds) as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCoursesListResponse;
            model.TrainingCourses.Should().BeEquivalentTo(mediatorResult.Courses, options=>options
                .Excluding(tc=>tc.ApprenticeshipFunding)
                .Excluding(tc=>tc.StandardDates)
            );
            model.Total.Should().Be(mediatorResult.Total);
            model.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCoursesListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
