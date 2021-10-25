﻿using AutoFixture.NUnit3;
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
            mockMediator.SetupMediatorResponseToReturnAsync<GetMoreDetailsQueryResult, GetMoreDetailsQuery>(getMoreDetailsQueryResult, 
                y => y.OpportunityId == opportunityId);
          
            var controllerResult = await opportunityController.MoreDetails(accountId, opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetMoreDetailsResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getMoreDetailsQueryResult.Opportunity.Id, response.Opportunity.Id);
            Assert.AreEqual(getMoreDetailsQueryResult.Sectors, response.Sectors);
            Assert.AreEqual(getMoreDetailsQueryResult.JobRoles, response.JobRoles);
            Assert.AreEqual(getMoreDetailsQueryResult.Levels, response.Levels);
        }

        [Test, MoqAutoData]
        public async Task Then_Given_Incorrect_Parameters_Returns_Not_Found(
            long accountId,
            int opportunityId,
            GetMoreDetailsQueryResult getMoreDetailsQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            getMoreDetailsQueryResult.Opportunity = null;

            mockMediator.SetupMediatorResponseToReturnAsync<GetMoreDetailsQueryResult, GetMoreDetailsQuery>(getMoreDetailsQueryResult,
                y => y.OpportunityId == opportunityId);

            var controllerResult = await opportunityController.MoreDetails(accountId, opportunityId);
            var result = controllerResult as NotFoundResult;
            

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
