using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
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
            GetSectorQueryResult getSectorQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<GetSectorQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getSectorQueryResult);

            var controllerResult = await opportunityController.Sector(pledgeId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetSectorResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getSectorQueryResult.Sectors, Is.EqualTo(response.Sectors));
            Assert.That(getSectorQueryResult.Opportunity.Id, Is.EqualTo(response.Opportunity.Id));
        }
    }
}