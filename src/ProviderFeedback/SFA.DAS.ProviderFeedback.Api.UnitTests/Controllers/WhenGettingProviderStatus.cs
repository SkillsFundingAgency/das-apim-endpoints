using System.Net;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Api.Controllers;
using SFA.DAS.ProviderFeedback.Api.Models;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProvider;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.ProviderAccounts
{
    public class WhenGettingProviderStatus
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
            int ukprn,
            bool result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]ProviderAccountsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderQuery>(c => c.Ukprn.Equals(ukprn)),
                CancellationToken.None)).ReturnsAsync(result);

            var actual = await controller.GetProviderStatus(ukprn) as OkObjectResult;
            
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as ProviderAccountResponse;
            actualModel.CanAccessService.Should().Be(result);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
            int ukprn,
            [Frozen] Mock<IMediator> mediator,
            [Greedy]ProviderAccountsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderQuery>(c => c.Ukprn.Equals(ukprn)),
                CancellationToken.None)).ThrowsAsync(new Exception("Error"));
            
            var actual = await controller.GetProviderStatus(ukprn) as StatusCodeResult;

            actual.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}