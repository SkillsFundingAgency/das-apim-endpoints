using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingAccepted
    {
        [Test, MoqAutoData]
        public async Task And_Result_Exists_Then_Returns_Ok_And_Result(
            int applicationId,
            GetAcceptedResult getAcceptedResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetAcceptedQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getAcceptedResult);

            var controllerResult = await applicationController.Accepted(applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getAcceptedResponse = okObjectResult.Value as GetAcceptedResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(getAcceptedResponse, Is.Not.Null);
        }

        [Test, MoqAutoData]
        public async Task And_Result_Doesnt_Exist_Then_Returns_NotFound(
            long accountId,
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetAcceptedQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetAcceptedResult)null);

            var controllerResult = await applicationController.Application(accountId, applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(notFoundResult, Is.Not.Null);
        }
    }
}