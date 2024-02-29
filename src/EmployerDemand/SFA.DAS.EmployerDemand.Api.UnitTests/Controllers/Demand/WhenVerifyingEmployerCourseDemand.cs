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
using SFA.DAS.EmployerDemand.Api.ApiRequests;
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenVerifyingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Id_Returned(
            Guid id,
            VerifyEmployerDemandCommandResult response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<VerifyEmployerDemandCommand>(command => 
                        command.Id == id
                    ),
                    It.IsAny<CancellationToken>())).ReturnsAsync(response);
            
            var controllerResult = await controller.VerifyCourseDemand(id) as CreatedResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            var actualModel = controllerResult.Value as VerifyCourseDemandResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Id.Should().Be(response.EmployerDemand.Id);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            Guid id,
            string errorContent,
            CreateCourseDemandRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<VerifyEmployerDemandCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.VerifyCourseDemand(id) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            Guid id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<VerifyEmployerDemandCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.VerifyCourseDemand(id) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}