using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingApprenticeshipsByUln
{
    [Test, MoqAutoData]
    public async Task Then_MatchingApprenticeshipsResponse_Returned_From_Mediator(
        string uln,
        GetUlnSupportApprenticeshipsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetUlnSupportApprenticeshipsQuery>(p => p.Uln == uln),
                It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.GetByUln(uln) as ObjectResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeOfType<GetUlnSupportApprenticeshipsQueryResult>()
            .Which.Should().BeEquivalentTo(mockQueryResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        string uln,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticeshipsController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.Is<GetUlnSupportApprenticeshipsQuery>(p => p.Uln == uln),
                It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.GetByUln(uln) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}