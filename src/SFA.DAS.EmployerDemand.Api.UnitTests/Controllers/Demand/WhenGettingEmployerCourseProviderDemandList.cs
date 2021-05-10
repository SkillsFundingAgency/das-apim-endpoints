using System;
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
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenGettingEmployerCourseProviderDemandList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Employer_Course_Demands_From_Mediator(
            int ukprn,
            int courseId,
            string location,
            int locationRadius,
            GetEmployerCourseProviderDemandQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEmployerCourseProviderDemandQuery>(query => 
                        query.Ukprn == ukprn &&
                        query.CourseId == courseId &&
                        query.LocationName == location &&
                        query.LocationRadius == locationRadius),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEmployerCourseProviderDemand(ukprn, courseId, location, locationRadius) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderEmployerCourseDemandListResponse;
            model!.TrainingCourse.Should().BeEquivalentTo((GetCourseListItem)mediatorResult.Course);
            model!.ProviderEmployerDemandDetailsList.Should().BeEquivalentTo(mediatorResult.EmployerCourseDemands.Select(response => (GetProviderEmployerDemandDetailsListItem)response));
            model!.Total.Should().Be(mediatorResult.Total);
            model!.TotalFiltered.Should().Be(mediatorResult.TotalFiltered);
            model!.Location.Should().BeEquivalentTo((GetLocationSearchResponseItem)mediatorResult.Location);
            model!.ProviderContactDetails.Should().BeEquivalentTo((GetProviderContactDetails)mediatorResult.ProviderDetail);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            int ukprn,
            int courseId,
            string location,
            int locationRadius,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEmployerCourseProviderDemandQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetEmployerCourseProviderDemand(ukprn, courseId, location, locationRadius) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}