using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    [TestFixture]
    public class WhenCallingWithdrawApplicationAfterAcceptance
    {
        [Test, MoqAutoData]
        public async Task And_Result_Exists_Then_Returns_Ok_And_Result(
           WithdrawApplicationAfterAcceptanceRequest request,
           int accountId,
           int applicationId,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] ApplicationsController applicationController)
        {
            var controllerResult = await applicationController.WithdrawApplicationAfterAcceptance(request, accountId, applicationId);
            var okObjectResult = controllerResult as OkResult;

            mockMediator.Verify(x => 
                x.Send(It.Is<WithdrawApplicationAfterAcceptanceCommand>(x => x.AccountId == accountId && 
                x.ApplicationId == applicationId &&
                x.UserId == request.UserId &&
                x.UserDisplayName == request.UserDisplayName), It.IsAny<CancellationToken>()));
            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(okObjectResult);
        }
    }
}
