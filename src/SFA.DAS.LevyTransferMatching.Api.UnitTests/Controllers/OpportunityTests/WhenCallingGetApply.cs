using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApply;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getApplyQueryResult.Opportunity.Id, response.Opportunity.Id);
            Assert.AreEqual(getApplyQueryResult.Sectors, response.Sectors);
            Assert.AreEqual(getApplyQueryResult.JobRoles, response.JobRoles);
            Assert.AreEqual(getApplyQueryResult.Levels, response.Levels);
        }
    }
}
