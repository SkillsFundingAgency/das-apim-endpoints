using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingGetApplications
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Applications_From_Mediator(GetApplicationsResult result, [Frozen]Mock<IMediator> mediator,
            [Greedy]ApplicationsController controller)
        {
            mediator.Setup(o => o.Send(It.IsAny<GetApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var controllerResult = await controller.GetApplications(1) as OkObjectResult;
            var applications = controllerResult.Value as GetApplicationsResult;
            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(applications);
            Assert.AreEqual(applications, result);
        }
    }
}
