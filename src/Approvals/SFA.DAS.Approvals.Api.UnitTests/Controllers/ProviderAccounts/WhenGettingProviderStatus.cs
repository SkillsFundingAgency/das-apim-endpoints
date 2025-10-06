using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.ProviderAccounts.Queries;
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ProviderAccounts
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

        [Test, MoqAutoData]
        public async Task Then_Get_Provider_Status_Mediator_Query_Is_Handled_And_Response_Returned(
            int ukprn,
            GetRoatpV2ProviderStatusQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderAccountsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderStatusQuery>(c => c.Ukprn.Equals(ukprn)), CancellationToken.None)).ReturnsAsync(result);

            var actual = await controller.GetProvider(ukprn) as OkObjectResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as ProviderAccountDetailsResponse;
            actualModel.ProviderStatusTypeId.Should().Be(result.ProviderStatusTypeId);
        }

        [Test, MoqAutoData]
        public async Task Then_Get_Provider_Status_If_Error_Then_Internal_Server_Error_Response_Returned(
            int ukprn,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProviderAccountsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderStatusQuery>(c => c.Ukprn.Equals(ukprn)),
                CancellationToken.None)).ThrowsAsync(new Exception("Error"));

            var actual = await controller.GetProvider(ukprn) as StatusCodeResult;

            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

    }
}