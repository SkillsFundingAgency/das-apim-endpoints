using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingGetPledge
    {
        [Test, MoqAutoData]
        public async Task And_Pledge_Exists_Then_Returns_Ok_And_Pledge(
            int pledgeId,
            GetPledgeResult getPledgeResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPledgeQuery>(y => y.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getPledgeResult);

            var controllerResult = await pledgeController.GetPledge(pledgeId);
            var okObjectResult = controllerResult as OkObjectResult;
            var pledgeDto = okObjectResult.Value as PledgeDto;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(pledgeDto);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Pledge_Doesnt_Exist_Then_Returns_NotFound(
            int pledgeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPledgeQuery>(y => y.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetPledgeResult)null);

            var controllerResult = await pledgeController.GetPledge(pledgeId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}