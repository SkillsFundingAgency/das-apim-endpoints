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
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.UserTests
{
    [TestFixture]
    public class WhenCallingGetOpportunityApply
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Accounts(
            int opportunityId,
            string userId,
            GetOpportunityApplyQueryResult getOpportunityApplyQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetOpportunityApplyQuery>(y => y.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getOpportunityApplyQueryResult);

            var controllerResult = await opportunityController.OpportunityApply(opportunityId, userId) as OkObjectResult;
            var response = controllerResult.Value as GetOpportunityApplyResponse;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(getOpportunityApplyQueryResult.Accounts.Count(), response.Accounts.Count());
        }
    }
}