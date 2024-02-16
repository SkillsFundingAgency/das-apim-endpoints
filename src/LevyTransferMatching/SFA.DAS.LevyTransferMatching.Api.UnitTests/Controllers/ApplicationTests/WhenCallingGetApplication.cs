using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingGetApplication
    {
        [Test, MoqAutoData]
        public async Task And_Application_Exists_Then_Returns_Ok_And_Application(
            long accountId,
            int applicationId,
            GetApplicationResult getApplicationResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationResult);

            var controllerResult = await applicationController.Application(accountId, applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getApplicationResponse = okObjectResult.Value as GetApplicationResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getApplicationResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Then_Returns_NotFound(
            long accountId,
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationResult)null);

            var controllerResult = await applicationController.Application(accountId, applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(notFoundResult);
        }
    }
}