using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingApplication
    {
        [Test, MoqAutoData]
        public async Task And_Mediator_Returns_Result_Then_Return_Response_And_Ok(
            int pledgeId,
            int applicationId,
            GetApplicationResult getApplicationResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationQuery>(y => y.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationResult);

            var controllerResult = await pledgeController.Application(pledgeId, applicationId);
            var createdResult = controllerResult as OkObjectResult;
            var getApplicationResponse = createdResult.Value as GetApplicationResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(getApplicationResponse);
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
                    It.Is<GetApplicationQuery>(y => y.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationResult)null);

            var controllerResult = await pledgeController.Application(pledgeId, applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}