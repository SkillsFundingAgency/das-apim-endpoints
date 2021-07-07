using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingGetPledges
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Pledges_From_Mediator(
            GetPledgesResult getPledgesResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<GetPledgesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getPledgesResult);

            var controllerResult = await pledgeController.GetPledges();
            var okObjectResult = controllerResult as OkObjectResult;
            var pledges = okObjectResult.Value as IEnumerable<PledgeDto>;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(pledges);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getPledgesResult.Count(), pledges.Count());
        }
    }
}