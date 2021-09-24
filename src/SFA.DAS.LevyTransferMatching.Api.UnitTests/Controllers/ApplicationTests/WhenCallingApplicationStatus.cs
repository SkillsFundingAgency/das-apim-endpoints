using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingApplicationStatus
    {
        [Test, MoqAutoData]
        public async Task And_Application_Exists_Then_Returns_Ok_And_Pledge(
            int applicationId,
            GetApplicationStatusResult getApplicationStatusResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetApplicationStatusQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationStatusResult);

            var controllerResult = await applicationController.ApplicationStatus(applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getApplicationStatusResponse = okObjectResult.Value as GetApplicationStatusResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getApplicationStatusResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Then_Returns_NotFound(
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetApplicationStatusQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationStatusResult)null);

            var controllerResult = await applicationController.ApplicationStatus(applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
        }
    }
}