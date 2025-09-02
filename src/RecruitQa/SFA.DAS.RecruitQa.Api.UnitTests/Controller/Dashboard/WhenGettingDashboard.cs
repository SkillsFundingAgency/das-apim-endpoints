using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Dashboard;
[TestFixture]
internal class WhenGettingDashboard
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        GetQaDashboardQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] DashboardController sut,
        CancellationToken token)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetQaDashboardQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var result = await sut.GetQaDashboard(token);
        var payload = (result as Ok<GetQaDashboardQueryResult>)?.Value;

        // assert
        mockMediator.Verify(mediator => mediator.Send(
            It.IsAny<GetQaDashboardQuery>(),
            It.IsAny<CancellationToken>()), Times.Once);
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(mediatorResponse, options => options.ExcludingMissingMembers());
    }

    
    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        GetQaDashboardQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] DashboardController sut,
        CancellationToken token)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetQaDashboardQuery>(),
                It.IsAny<CancellationToken>()))
           .Throws<InvalidOperationException>();

        var actual = await sut.GetQaDashboard(token);

        actual.Should().NotBeNull();
        actual.Should().BeOfType<ProblemHttpResult>();
    }
}
