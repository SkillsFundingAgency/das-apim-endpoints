using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetMoreDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    public class WhenCallingGetMoreDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Response(
            long accountId,
            int opportunityId,
            GetMoreDetailsQueryResult getMoreDetailsQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetMoreDetailsQuery>(y => y.OpportunityId == opportunityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getMoreDetailsQueryResult);

            var controllerResult = await opportunityController.MoreDetails(accountId, opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetMoreDetailsResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getMoreDetailsQueryResult.Opportunity.Id, Is.EqualTo(response.Opportunity.Id));
            Assert.That(getMoreDetailsQueryResult.Sectors, Is.EquivalentTo(response.Sectors));
            Assert.That(getMoreDetailsQueryResult.JobRoles, Is.EquivalentTo(response.JobRoles));
            Assert.That(getMoreDetailsQueryResult.Levels, Is.EqualTo(response.Levels));
        }
    }
}
