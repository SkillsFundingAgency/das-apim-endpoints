using System.Net;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingCohortAndSupportStatus
{
    [Test, MoqAutoData]
    public async Task Then_CohortSupportStatusQueryResponse_Returned_From_Mediator(
        long id,
        GetCohortSupportApprenticeshipsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CohortsController sut)
    {
        mockMediator.Setup(x => x.Send(It.Is<GetCohortSupportApprenticeshipsQuery>(p=>p.CohortId == id), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(id) as ObjectResult;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetCohortSupportApprenticeshipsQueryResult>()
                .Which.Should().BeEquivalentTo(mockQueryResult);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_CohortSupportStatusQueryResponse_IsNull_result_Is_NotFound(
        long id,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CohortsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetCohortSupportApprenticeshipsQuery>(p => p.CohortId == id),
                It.IsAny<CancellationToken>())).ReturnsAsync((GetCohortSupportApprenticeshipsQueryResult)null);

        var actual = await sut.Get(id) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        long id,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CohortsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetCohortSupportApprenticeshipsQuery>(p => p.CohortId == id),
                It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(id) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}