using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
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
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetDetailQuery>(y => y.OpportunityId == opportunityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getDetailQueryResult);

            var controllerResult = await opportunityController.Detail(opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetDetailResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(getDetailQueryResult.Opportunity.Id, Is.EqualTo(response.Opportunity.Id));
            Assert.That(getDetailQueryResult.Sectors, Is.EqualTo(response.Sectors));
            Assert.That(getDetailQueryResult.JobRoles, Is.EqualTo(response.JobRoles));
            Assert.That(getDetailQueryResult.Levels, Is.EqualTo(response.Levels));
        }
    }
}
