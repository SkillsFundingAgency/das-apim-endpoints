using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests
{
    public class WhenCallingContactDetails
    {
        [Test, MoqAutoData]
        public async Task And_Opportunity_Exists_Then_Returns_Ok_And_Pledge(
            int opportunityId,
            GetContactDetailsResult getContactDetailsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetContactDetailsQuery>(y => y.OpportunityId == opportunityId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getContactDetailsResult);

            var controllerResult = await opportunityController.ContactDetails(opportunityId);
            var okObjectResult = controllerResult as OkObjectResult;
            var getContactDetailsResponse = okObjectResult.Value as GetContactDetailsResponse;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getContactDetailsResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Opportunity_Doesnt_Exist_Then_Returns_NotFound(
            int opportunityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OpportunityController opportunityController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetContactDetailsQuery>(y => y.OpportunityId == opportunityId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetContactDetailsResult)null);

            var controllerResult = await opportunityController.ContactDetails(opportunityId);
            var notFoundResult = controllerResult as NotFoundResult;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(notFoundResult);
        }
    }
}