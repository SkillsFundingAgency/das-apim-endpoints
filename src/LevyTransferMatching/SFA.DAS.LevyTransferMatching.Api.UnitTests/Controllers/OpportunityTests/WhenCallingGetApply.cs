using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApply;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    public class WhenCallingGetApply
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Response(
            long accountId,
            int opportunityId,
            GetApplyQueryResult getApplyQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplyQuery>(y => y.OpportunityId == opportunityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplyQueryResult);

            var controllerResult = await opportunityController.Apply(accountId, opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetApplyResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getApplyQueryResult.Opportunity.Id, Is.EqualTo(response.Opportunity.Id));
            Assert.That(getApplyQueryResult.Sectors, Is.EqualTo(response.Sectors));
            Assert.That(getApplyQueryResult.JobRoles, Is.EqualTo(response.JobRoles));
            Assert.That(getApplyQueryResult.Levels, Is.EqualTo(response.Levels));
        }
    }
}
