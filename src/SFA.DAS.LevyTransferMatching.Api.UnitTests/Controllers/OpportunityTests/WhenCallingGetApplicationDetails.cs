using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApplicationDetails;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    [TestFixture]
    public class WhenCallingGetApplicationDetails
    {
        [Test, MoqAutoData]
        public async Task And_Opportunity_Exists_Then_Returns_Ok_Pledge_And_Standards(
           long accountId,
           int opportunityId,
           GetApplicationDetailsQueryResult getApplicationDetailsQueryResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationDetailsQuery>(y => y.OpportunityId == opportunityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationDetailsQueryResult);

            var controllerResult = await opportunityController.GetApplicationDetails(accountId, opportunityId, default);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as ApplicationDetailsResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Opportunity_Doesnt_Exist_Then_Returns_NotFound(
            int accountId,
            int opportunityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationDetailsQuery>(y => y.OpportunityId == opportunityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetApplicationDetailsQueryResult());

            var controllerResult = await opportunityController.GetApplicationDetails(accountId, opportunityId, default);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}