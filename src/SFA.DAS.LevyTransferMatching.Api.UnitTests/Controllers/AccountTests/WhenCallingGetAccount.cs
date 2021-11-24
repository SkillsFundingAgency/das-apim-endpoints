using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.AccountTests
{
    public class WhenCallingGetAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_The_Mediator(string encodedAccountId, GetAccountResult result, [Frozen] Mock<IMediator> mediator, [Greedy] AccountController controller)
        {
            mediator.SetupMediatorResponseToReturnAsync<GetAccountResult, GetAccountQuery>(result, o => o.EncodedAccountId == encodedAccountId);

            var controllerResult = await controller.GetAccount(encodedAccountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.OK);

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.OK);

            var model = controllerResult.Value as AccountDto;
            Assert.IsNotNull(model);
            Assert.AreEqual(result.Account.RemainingTransferAllowance, model.RemainingTransferAllowance);
        }

        [Test, MoqAutoData]
        public async Task When_Given_Invalid_Account_Id_Returns_NotFound(string encodedAccountId, GetAccountResult result, [Frozen] Mock<IMediator> mediator, [Greedy] AccountController controller)
        {
            result.Account = null;

            mediator.SetupMediatorResponseToReturnAsync<GetAccountResult, GetAccountQuery>(result, o => o.EncodedAccountId == encodedAccountId);

            var controllerResult = await controller.GetAccount(encodedAccountId) as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}