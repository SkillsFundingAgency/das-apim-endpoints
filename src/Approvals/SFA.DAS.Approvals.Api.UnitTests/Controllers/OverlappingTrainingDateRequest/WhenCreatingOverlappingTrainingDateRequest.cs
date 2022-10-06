using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.OverlappingTrainingDateRequest;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest
{
    public class WhenCreatingOverlappingTrainingDateRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Command_To_Create_Ovelapping_Training_Date_Request(
          CreateOverlappingTrainingDateRequest request,
          CreateOverlappingTrainingDateResult result,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] OverlappingTrainingDateRequestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<CreateOverlappingTrainingDateRequestCommand>(x => 
                    x.ProviderId == request.ProviderId 
                    && x.DraftApprenticeshipId == request.DraftApprenticeshipId
                    && x.UserInfo == request.UserInfo),
                    It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var controllerResult = await controller.CreateOverlappingTrainingDate(request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
