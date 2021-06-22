using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingGetPledge
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Pledge_From_Mediator(
            Pledge pledge,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            var encodedPledgeId = pledge.EncodedPledgeId;

            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPledgesQuery>(x => x.EncodedId == encodedPledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPledgesResult(new Pledge[] { pledge }));

            var controllerResult = await pledgeController.GetPledge(encodedPledgeId);
            var okObjectResult = controllerResult as OkObjectResult;
            var pledgeDto = okObjectResult.Value as PledgeDto;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(pledgeDto);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(encodedPledgeId, pledgeDto.EncodedPledgeId);
        }

        [Test, MoqAutoData]
        public async Task Then_Doesnt_Get_Pledge_From_Mediator(
            string encodedPledgeId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetPledgesQuery>(x => x.EncodedId == encodedPledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPledgesResult(new Pledge[] { }));

            var controllerResult = await pledgeController.GetPledge(encodedPledgeId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}