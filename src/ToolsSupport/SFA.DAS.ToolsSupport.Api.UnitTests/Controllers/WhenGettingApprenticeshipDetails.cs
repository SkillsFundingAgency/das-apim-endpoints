using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingApprenticeshipDetails
{
    [Test, MoqAutoData]
    public async Task Then_ApprenticeshipResponse_Returned_From_Mediator(
        long id,
        GetApprenticeshipQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetApprenticeshipQuery>(p => p.Id == id),
                It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.GetById(id) as ObjectResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeOfType<GetApprenticeshipQueryResult>()
            .Which.Should().BeEquivalentTo(mockQueryResult);
    }

    [Test, MoqAutoData]
    public async Task Then_ApprenticeDetailsResponse_IsNull_result_Is_NotFound(
        long id,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetApprenticeshipQuery>(p => p.Id == id),
                It.IsAny<CancellationToken>())).ReturnsAsync((GetApprenticeshipQueryResult)null);

        var actual = await sut.GetById(id) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        long id,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetApprenticeshipQuery>(p => p.Id == id),
                It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.GetById(id) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}