﻿using System.Threading;
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
    public class WhenCallingApplication
    {
        [Test, MoqAutoData]
        public async Task And_Application_Exists_Then_Returns_Ok_And_Application(
            int applicationId,
            GetApplicationResult getApplicationResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator.SetupMediatorResponseToReturnAsync<GetApplicationResult, GetApplicationQuery>(getApplicationResult, o => o.ApplicationId == applicationId);
           
            var controllerResult = await applicationController.Application(applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getApplicationResponse = okObjectResult.Value as GetApplicationResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getApplicationResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Then_Returns_NotFound(
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator.SetupMediatorResponseToReturnAsync<GetApplicationResult, GetApplicationQuery>(null, o => o.ApplicationId == applicationId);
          
            var controllerResult = await applicationController.Application(applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
        }
    }
}