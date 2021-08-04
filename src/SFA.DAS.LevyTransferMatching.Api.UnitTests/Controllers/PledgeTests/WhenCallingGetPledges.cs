using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
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

            var actionResult = await pledgeController.GetPledges();
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var pledges = value as GetPledgesResponse;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(value);
            Assert.IsNotNull(pledges);

            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getPledgesResult.Count(), pledges.Count());
        }
    }
}