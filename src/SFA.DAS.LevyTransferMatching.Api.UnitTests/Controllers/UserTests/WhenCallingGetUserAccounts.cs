using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.UserTests
{
    [TestFixture]
    public class WhenCallingGetUserAccounts
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Accounts(
            string userId,
            GetUserAccountsResult getUserAccountsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UserController userController)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetUserAccountsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getUserAccountsResult);

            var controllerResult = await userController.GetUserAccounts(userId) as OkObjectResult;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int)HttpStatusCode.OK);

            var model = controllerResult.Value as IEnumerable<UserAccountDto>;

            Assert.AreEqual(getUserAccountsResult.UserAccounts.Count(), model.Count());
        }
    }
}