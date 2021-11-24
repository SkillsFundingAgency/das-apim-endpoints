using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    class WhenCallingGetDetail
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Response(
            int opportunityId,
            GetDetailQueryResult getDetailQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator.SetupMediatorResponseToReturnAsync<GetDetailQueryResult, GetDetailQuery>(getDetailQueryResult, y => y.OpportunityId == opportunityId);
        
            var controllerResult = await opportunityController.Detail(opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetDetailResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getDetailQueryResult.Opportunity.Id, response.Opportunity.Id);
            Assert.AreEqual(getDetailQueryResult.Sectors, response.Sectors);
            Assert.AreEqual(getDetailQueryResult.JobRoles, response.JobRoles);
            Assert.AreEqual(getDetailQueryResult.Levels, response.Levels);
        }

        [Test, MoqAutoData]
        public async Task Then_When_Given_Incorrect_Parameters_Returns_Not_Found(
            int opportunityId,
            GetDetailQueryResult getDetailQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            getDetailQueryResult.Opportunity = null;

            mockMediator.SetupMediatorResponseToReturnAsync<GetDetailQueryResult, GetDetailQuery>(getDetailQueryResult, y => y.OpportunityId == opportunityId);

            var controllerResult = await opportunityController.Detail(opportunityId);
            var result = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
