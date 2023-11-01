using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.ApprenticesControllerTests;

public class GetApprenticeTests
{
    [Test, MoqAutoData]
    public async Task GetApprentice_RecordNotFound_ReturnsNotFoundResponse(
        [Frozen] Mock<IAanHubRestApiClient> clientMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        clientMock.Setup(m => m.GetApprenticeMember(apprenticeId, cancellationToken)).ReturnsAsync(new RestEase.Response<GetApprenticeResult>(null, new HttpResponseMessage(HttpStatusCode.NotFound), () => new GetApprenticeResult()));

        var result = await sut.GetApprentice(apprenticeId, cancellationToken);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_RecordFound_ReturnsOkResponse(
        [Frozen] Mock<IAanHubRestApiClient> clientMock,
        [Greedy] ApprenticesController sut,
        GetApprenticeResult expectedResult,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        clientMock.Setup(m => m.GetApprenticeMember(apprenticeId, cancellationToken)).ReturnsAsync(new RestEase.Response<GetApprenticeResult>(null, new HttpResponseMessage(HttpStatusCode.OK), () => expectedResult));

        var result = await sut.GetApprentice(apprenticeId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.As<GetApprenticeResult?>().Should().BeEquivalentTo(expectedResult);
    }
}
