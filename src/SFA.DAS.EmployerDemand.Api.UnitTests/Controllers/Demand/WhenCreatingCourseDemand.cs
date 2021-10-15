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
using SFA.DAS.EmployerDemand.Api.ApiRequests;
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenCreatingCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Id_Returned(
            Guid returnId,
            CreateCourseDemandRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterDemandCommand>(command => 
                        command.Id == request.Id
                        && command.OrganisationName.Equals(request.OrganisationName)
                        && command.ContactEmailAddress.Equals(request.ContactEmailAddress)
                        && command.NumberOfApprentices.Equals(request.NumberOfApprentices)
                        && command.Lat.Equals(request.LocationItem.Location.GeoPoint.First())
                        && command.Lon.Equals(request.LocationItem.Location.GeoPoint.Last())
                        && command.LocationName.Equals(request.LocationItem.Name)
                        && command.CourseId.Equals(request.TrainingCourse.Id)
                        && command.CourseTitle.Equals(request.TrainingCourse.Title)
                        && command.CourseLevel.Equals(request.TrainingCourse.Level)
                        && command.CourseRoute.Equals(request.TrainingCourse.Route)
                        && command.ConfirmationLink.Equals(request.ResponseUrl)
                        && command.StopSharingUrl.Equals(request.StopSharingUrl)
                        && command.StartSharingUrl.Equals(request.StartSharingUrl)
                        && command.ExpiredCourseDemandId.Equals(request.ExpiredCourseDemandId)
                        && command.EntryPoint.Equals(request.EntryPoint)
                    ),
                    It.IsAny<CancellationToken>())).ReturnsAsync((returnId));
            
            var controllerResult = await controller.CreateCourseDemand(request) as CreatedResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().Be(returnId);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Null_Is_Returned_Then_Conflict_returned(
            Guid returnId,
            CreateCourseDemandRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterDemandCommand>(command => 
                        command.Id == request.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync((Guid?)null);
            
            var controllerResult = await controller.CreateCourseDemand(request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            string errorContent,
            CreateCourseDemandRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<RegisterDemandCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.CreateCourseDemand(request) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            CreateCourseDemandRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<RegisterDemandCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.CreateCourseDemand(request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}