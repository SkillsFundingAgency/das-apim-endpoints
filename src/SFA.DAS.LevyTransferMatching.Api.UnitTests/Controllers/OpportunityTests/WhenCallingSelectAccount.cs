using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSelectAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    public class WhenCallingSelectAccount
    {
        [Test, MoqAutoData]
        public async Task And_Given_Valid_Parameters_Returns_Successful_Response(int opportunityId, string userId, GetSelectAccountQueryResult result, [Frozen] Mock<IMediator> mediator,
            [Greedy] OpportunityController controller)
        {
            mediator.SetupMediatorResponseToReturnAsync<GetSelectAccountQueryResult, GetSelectAccountQuery>(result, o => o.UserId == userId);

            var controllerResponse = await controller.SelectAccount(opportunityId, userId);
            var response = controllerResponse as OkObjectResult;
            var responseValue = response.Value as GetSelectAccountResponse;

            Assert.IsNotNull(controllerResponse);
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(result.Accounts.Count(), responseValue.Accounts.Count());
        }
    }
}
