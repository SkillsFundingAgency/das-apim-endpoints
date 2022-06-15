using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.Account
{
    public class WhenCallingGetAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            string encodedAccountId,
            GetAccountResult getAccountResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountController accountController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<GetAccountQuery>(y => y.EncodedAccountId.Equals(encodedAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(getAccountResult);

            var controllerResult = await accountController.GetAccount(encodedAccountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.OK);

            var model = controllerResult.Value as AccountDto;
            Assert.IsNotNull(model);
            Assert.AreEqual(getAccountResult.Account.RemainingTransferAllowance, model.RemainingTransferAllowance);
        }
    }
}