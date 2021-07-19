using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    public class WhenCallingGetSector
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Response(
            int pledgeId,
            string postcode,
            GetSectorQueryResult getSectorQueryResult,
            GetOpportunityResult getOpportunityResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetSectorQuery>(y => y.Postcode == postcode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getSectorQueryResult);

            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetOpportunityQuery>(y => y.OpportunityId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getOpportunityResult);

            var controllerResult = await opportunityController.Sector(pledgeId, postcode);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetSectorResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getSectorQueryResult.Sectors, response.Sectors);
            Assert.AreEqual(getSectorQueryResult.Location, response.Location);
            Assert.AreEqual(getOpportunityResult.Id, response.Opportunity.Id);
        }
    }
}
