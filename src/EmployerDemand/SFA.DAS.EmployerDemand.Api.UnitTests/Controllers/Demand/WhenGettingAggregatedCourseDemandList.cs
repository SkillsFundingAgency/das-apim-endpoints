using System;
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
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenGettingAggregatedCourseDemandList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_And_Demands_From_Mediator(
            int ukprn,
            int courseId,
            string location,
            int locationRadius,
            List<string> routes,
            GetAggregatedCourseDemandListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAggregatedCourseDemandListQuery>(query => 
                        query.Ukprn == ukprn &&
                        query.CourseId == courseId &&
                        query.LocationName == location &&
                        query.LocationRadius == locationRadius),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAggregatedCourseDemandList(ukprn, courseId, location, locationRadius, routes) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAggregatedCourseDemandListResponse;
            model!.TrainingCourses.Should().BeEquivalentTo(mediatorResult.Courses.Select(item => (GetCourseListItem)item));
            model!.AggregatedCourseDemands.Should().BeEquivalentTo(mediatorResult.AggregatedCourseDemands.Select(response => (GetAggregatedCourseDemandSummary)response));
            model!.Total.Should().Be(mediatorResult.Total);
            model!.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
            model!.Location.Should().BeEquivalentTo((GetLocationSearchResponseItem)mediatorResult.LocationItem);
            model!.Routes.Should().BeEquivalentTo(mediatorResult.Routes);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            int courseId,
            string location,
            int locationRadius,
            List<string> routes,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAggregatedCourseDemandListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAggregatedCourseDemandList(ukprn, courseId, location, locationRadius, routes) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}