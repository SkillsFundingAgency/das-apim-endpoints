using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.UserTests
{
    [TestFixture]
    public class WhenCallingGetSelectAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Accounts(
            int opportunityId,
            string userId,
            GetSelectAccountQueryResult getSelectAccountQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetSelectAccountQuery>(y => y.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getSelectAccountQueryResult);

            var controllerResult = await opportunityController.SelectAccount(opportunityId, userId) as OkObjectResult;
            var response = controllerResult.Value as GetSelectAccountResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(controllerResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(response, Is.Not.Null);
            Assert.That(getSelectAccountQueryResult.Accounts.Count(), Is.EqualTo(response.Accounts.Count()));
        }
    }
}