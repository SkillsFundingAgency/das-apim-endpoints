using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class ClosePledgeTests
    {
        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_Closed_Pledge_Status(
           int pledgeId,
           ClosePledgeCommandResult closePledgeResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(closePledgeResult);

            var controllerResponse = await pledgeController.ClosePledge(pledgeId);
         
            var okObjectResult = controllerResponse as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var response = okObjectResult.Value as GetClosePledgeResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Updated);
        }

        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_BadRequest(
          int pledgeId,
          ClosePledgeCommandResult closePledgeResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] PledgeController pledgeController)
        {
            closePledgeResult = null;
            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(closePledgeResult);

            var controllerResponse = await pledgeController.ClosePledge(pledgeId);

            var badRequestObjectResult = controllerResponse as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);
            var response = badRequestObjectResult.Value as GetClosePledgeResponse;
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Updated);            
        }

        [Test, MoqAutoData]
        public async Task ClosePledge_Returns_Error(
          int pledgeId,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<ClosePledgeCommand>((x) => x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .Throws(new Exception());
            
            var controllerResponse = await pledgeController.ClosePledge(pledgeId);

            var exception = controllerResponse as StatusCodeResult;
            Assert.IsNotNull(exception);
            Assert.AreEqual(500,exception.StatusCode);
        }
    }
}
