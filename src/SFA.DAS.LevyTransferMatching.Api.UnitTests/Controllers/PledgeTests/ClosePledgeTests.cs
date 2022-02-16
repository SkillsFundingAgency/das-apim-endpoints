using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class ClosePledgeTests
    {
        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_Closed_Pledge_Status(
           int pledgeId,
           SetClosePledgeRequest closePledgeRequest,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] PledgeController pledgeController)
        {
            var closePledgeResult = new ClosePledgeCommandResult{
                 ErrorContent = string.Empty,
                 StatusCode = HttpStatusCode.OK
            };

            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(closePledgeResult);

            var controllerResponse = await pledgeController.ClosePledge(pledgeId, closePledgeRequest);
         
            var statusResult = controllerResponse as StatusCodeResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual((int)HttpStatusCode.OK, statusResult.StatusCode);
        }

        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_NotFound(
          int pledgeId,
          SetClosePledgeRequest closePledgeRequest,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] PledgeController pledgeController)
        {
            var closePledgeResult = new ClosePledgeCommandResult
            {
                ErrorContent = string.Empty,
                StatusCode = HttpStatusCode.NotFound
            };
            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(closePledgeResult);

            var controllerResponse = await pledgeController.ClosePledge(pledgeId, closePledgeRequest);

            var statusResult = controllerResponse as StatusCodeResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, statusResult.StatusCode);            
        }

        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_Error(
          int pledgeId,
           SetClosePledgeRequest closePledgeRequest,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .Throws(new InvalidOperationException());
            
            var controllerResponse = await pledgeController.ClosePledge(pledgeId, closePledgeRequest);

            var exception = controllerResponse as StatusCodeResult;
            Assert.IsNotNull(exception);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, exception.StatusCode);
        }
    }
}
