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
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.ProviderInterest
{
    public class WhenCreatingProviderInterests
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator(
            Guid returnId,
            CreateProviderInterestsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderInterestController controller)
        {
            CreateProviderInterestsCommand actualCommand = null;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateProviderInterestsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnId)
                .Callback((CreateProviderInterestsCommand command, CancellationToken token) => actualCommand = command);
            
            var controllerResult = await controller.CreateProviderInterests(request) as CreatedResult;

            actualCommand.Should().BeEquivalentTo((CreateProviderInterestsCommand)request);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().Be(returnId);
        }

        [Test, MoqAutoData]
        public async Task And_HttpRequestContentException_Then_Http_Bad_Request_Is_Returned_With_Error(
            string errorContent,
            CreateProviderInterestsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderInterestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateProviderInterestsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.CreateProviderInterests(request) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Http_Bad_Request_Is_Returned(
            CreateProviderInterestsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderInterestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateProviderInterestsCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.CreateProviderInterests(request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}