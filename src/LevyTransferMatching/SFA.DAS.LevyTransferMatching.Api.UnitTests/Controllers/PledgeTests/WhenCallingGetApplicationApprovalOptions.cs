using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class WhenCallingGetApplicationApprovalOptions
    {
        [Test, MoqAutoData]
        public async Task And_Mediator_Returns_Result_Then_Return_Response_And_Ok(
            int pledgeId,
            int applicationId,
            GetApplicationApprovalOptionsQueryResult getApplicationApprovalOptionsQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationApprovalOptionsQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationApprovalOptionsQueryResult);

            var controllerResult = await pledgeController.ApplicationApprovalOptions(pledgeId, applicationId);
            var createdResult = controllerResult as OkObjectResult;
            var getApplicationApprovalOptionsResponse = createdResult.Value as GetApplicationApprovalOptionsResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(getApplicationApprovalOptionsResponse);
            Assert.AreEqual(createdResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Doesnt_Return_Result_Then_Return_NotFound(
            int pledgeId,
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationApprovalOptionsQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationApprovalOptionsQueryResult)null);

            var controllerResult = await pledgeController.Application(pledgeId, applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
