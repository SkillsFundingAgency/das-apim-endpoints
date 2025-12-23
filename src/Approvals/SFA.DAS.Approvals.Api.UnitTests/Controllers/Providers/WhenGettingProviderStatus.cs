using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.Providers.Queries;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProviderStatus
    {        
        [Test, MoqAutoData]
        public async Task Then_Get_Provider_Status_Mediator_Query_Is_Handled_And_Response_Returned(
            int ukprn,
            GetRoatpV2ProviderStatusQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderStatusQuery>(c => c.Ukprn.Equals(ukprn)), CancellationToken.None)).ReturnsAsync(result);

            var actual = await controller.GetProvider(ukprn) as OkObjectResult;

            actual.Should().NotBeNull();
            var actualModel = actual.Value as ProviderAccountDetailsResponse;
            actualModel.ProviderStatusTypeId.Should().Be(result.ProviderStatusTypeId);
        }

        [Test, MoqAutoData]
        public async Task Then_Get_Provider_Status_If_Error_Then_Internal_Server_Error_Response_Returned(
            int ukprn,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ProvidersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetRoatpV2ProviderStatusQuery>(c => c.Ukprn.Equals(ukprn)),
                CancellationToken.None)).ThrowsAsync(new Exception("Error"));

            var actual = await controller.GetProvider(ukprn) as StatusCodeResult;

            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

    }
}