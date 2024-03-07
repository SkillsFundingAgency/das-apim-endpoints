using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationAcceptance;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingPostApplication
    {
        [Test, MoqAutoData]
        public async Task And_Operation_Is_Success_Returns_NoContent(
            long accountId,
            int applicationId,
            SetApplicationAcceptanceRequest setApplicationAcceptanceRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationsController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<SetApplicationAcceptanceCommand>(y => y.AccountId == accountId && y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var actionResult = await applicationsController.Application(accountId, applicationId, setApplicationAcceptanceRequest);
            var noContentResult = actionResult as NoContentResult;

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(noContentResult, Is.Not.Null);
        }

        [Test, MoqAutoData]
        public async Task And_Operation_Is_Unsuccessful_Returns_BadRequest(
            long accountId,
            int applicationId,
            SetApplicationAcceptanceRequest setApplicationAcceptanceRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationsController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<SetApplicationAcceptanceCommand>(y => y.AccountId == accountId && y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var actionResult = await applicationsController.Application(accountId, applicationId, setApplicationAcceptanceRequest);
            var badRequestResult = actionResult as BadRequestResult;

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(badRequestResult, Is.Not.Null);
        }
    }
}