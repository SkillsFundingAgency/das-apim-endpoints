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
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenGettingACourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Location_From_Mediator(
            int courseId,
            string locationName,
            GetRegisterDemandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetRegisterDemandQuery>(c => c.CourseId.Equals(courseId) && c.LocationName.Equals(locationName)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Create(courseId, locationName) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo((GetCourseListItem)mediatorResult.Course);
            model.Location.Should().BeEquivalentTo((GetLocationSearchResponseItem)mediatorResult.Location);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int courseId,
            string locationName,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetRegisterDemandQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Create(courseId, locationName) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}