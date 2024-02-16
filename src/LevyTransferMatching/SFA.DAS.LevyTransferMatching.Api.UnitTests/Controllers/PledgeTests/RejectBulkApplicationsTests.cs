using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplications;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class RejectBulkApplicationsTests
    {
        [Test, MoqAutoData]
        public async Task Reject_Applications_Performs_Bulk_Applications_Rejection_With_Status_Ok(
          int accountId,
          int pledgeId,
          RejectApplicationsRequest applicationRejectRequest,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<RejectApplicationsCommand>((x) => (x.PledgeId == pledgeId) && (x.AccountId == accountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            applicationRejectRequest.ApplicationsToReject = new List<int> {5};

            var controllerResponse = await pledgeController.RejectApplications(accountId, pledgeId, applicationRejectRequest);

            var statusResult = controllerResponse as StatusCodeResult;
            Assert.That(statusResult, Is.Not.Null);
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }
    }
}
