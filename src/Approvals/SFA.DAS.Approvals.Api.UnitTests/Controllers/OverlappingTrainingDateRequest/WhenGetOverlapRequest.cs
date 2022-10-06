using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest
{
    public class WhenGetOverlapRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Query_To_Get_Overlap_Request(long apprenticeshipId,
            GetOverlapRequestResult result,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OverlappingTrainingDateRequestController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetOverlapRequestQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var controllerResult = await controller.GetOverlapRequest(apprenticeshipId) as ObjectResult;

            Assert.IsNotNull(controllerResult);

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}