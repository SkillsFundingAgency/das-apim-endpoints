using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingGetApplicationApproved
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_PledgeReference_From_Mediator(
            long accountId,
            int pledgeId,
            int applicationId,
            GetApplicationApprovedQueryResult getApplicationApprovedQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetApplicationApprovedQuery>((x) => x.ApplicationId == applicationId && x.PledgeId == pledgeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getApplicationApprovedQueryResult);

            var controllerResult = await pledgeController.ApplicationApproved(accountId, pledgeId, applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetApplicationApprovedResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(response.EmployerAccountName, getApplicationApprovedQueryResult.EmployerAccountName);
        }
    }
}
