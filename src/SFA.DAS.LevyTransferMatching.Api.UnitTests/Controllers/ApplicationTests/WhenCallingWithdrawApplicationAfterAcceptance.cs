using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
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
           long accountId,
           int applicationId,
           WithdrawApplicationAfterAcceptanceCommandResult result,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] ApplicationsController applicationController)
        {
            result.StatusCode = System.Net.HttpStatusCode.OK;

            mockMediator
                .Setup(x => x.Send(It.Is<WithdrawApplicationAfterAcceptanceCommand>(y => y.UserId == request.UserId &&
                                                                                        y.UserDisplayName == y.UserDisplayName &&
                                                                                        y.AccountId == accountId &&
                                                                                        y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                                                                                    .ReturnsAsync(result);

            var controllerResult = await applicationController.WithdrawApplicationAfterAcceptance(request, accountId, applicationId);
            var okObjectResult = controllerResult as OkResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
        }
    }
}
