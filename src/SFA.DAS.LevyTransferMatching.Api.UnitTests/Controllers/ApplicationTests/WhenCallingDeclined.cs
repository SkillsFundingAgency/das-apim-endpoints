﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingDeclined
    {
        [Test, MoqAutoData]
        public async Task And_Result_Exists_Then_Returns_Ok_And_Result(
            int applicationId,
            GetDeclinedResult getDeclinedResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetDeclinedQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getDeclinedResult);

            var controllerResult = await applicationController.Declined(applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getDeclinedResponse = okObjectResult.Value as GetDeclinedResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getDeclinedResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Result_Doesnt_Exist_Then_Returns_NotFound(
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetDeclinedQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetDeclinedResult)null);

            var controllerResult = await applicationController.Application(applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
        }
    }
}