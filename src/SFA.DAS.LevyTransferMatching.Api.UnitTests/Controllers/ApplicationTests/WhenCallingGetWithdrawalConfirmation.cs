using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    [TestFixture]
    public class WhenCallingGetWithdrawalConfirmation
    {
        [Test, MoqAutoData]
        public async Task And_Result_Exists_Then_Returns_Ok_And_Result(
           long accountId,
           int applicationId,
           GetWithdrawalConfirmationQueryResult result,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetWithdrawalConfirmationQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var controllerResult = await applicationController.GetWithdrawalConfirmation(accountId, applicationId);
            var okObjectResult = controllerResult as OkObjectResult;
            var response = okObjectResult.Value as GetWithdrawalConfirmationResponse;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(response);
        }

        [Test, MoqAutoData]
        public async Task And_Result_Doesnt_Exist_Then_Returns_NotFound(
            long accountId,
            int applicationId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationsController applicationController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetWithdrawalConfirmationQuery>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetWithdrawalConfirmationQueryResult)null);

            var controllerResult = await applicationController.GetWithdrawalConfirmation(accountId, applicationId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(notFoundResult);
        }
    }
}
