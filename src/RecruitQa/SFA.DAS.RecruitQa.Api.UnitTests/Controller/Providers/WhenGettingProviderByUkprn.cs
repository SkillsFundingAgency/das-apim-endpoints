using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.Application.Provider.GetProvider;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Providers;

[TestFixture]
internal class WhenGettingProviderByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Handled_And_Response_Returned(
        int ukprn,
        GetProviderQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderQuery>(c => c.Ukprn.Equals(ukprn)),
            CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.GetProvider(ukprn);
        var payload = (actual as Ok<GetProviderResponse>)?.Value;

        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(result, options => options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_Internal_Server_Error_Response_Returned(
        int ukprn,
        GetProviderQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ProvidersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetProviderQuery>(c => c.Ukprn.Equals(ukprn)),
           CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.GetProvider(ukprn);
        actual.Should().BeOfType<ProblemHttpResult>();
    }
}
