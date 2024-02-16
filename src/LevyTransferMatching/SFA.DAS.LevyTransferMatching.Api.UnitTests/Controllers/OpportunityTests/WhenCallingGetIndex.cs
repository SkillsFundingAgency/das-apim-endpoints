﻿using AutoFixture;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    [TestFixture]
    public class WhenCallingGetIndex
    {
        private readonly Fixture _fixture = new Fixture();

        [Test, MoqAutoData]
        public async Task Returns_Ok_And_Response(
            GetIndexQueryResult getIndexQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController
            )
        {
            var sectorsList = _fixture.Create<IEnumerable<string>>();
            mockMediator
                .Setup(x => x.Send(
                    It.IsAny<GetIndexQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getIndexQueryResult);

            var controllerResult = await opportunityController.GetIndex(sectorsList);
            var okObjectResult = controllerResult as OkObjectResult;
            var responseObject = okObjectResult.Value as GetIndexResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(responseObject);
            Assert.IsNotNull(responseObject.Opportunities);
            Assert.IsNotNull(responseObject.Sectors);
            Assert.IsNotNull(responseObject.JobRoles);
            Assert.IsNotNull(responseObject.Levels);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
        }
    }
}
