using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers.MembersControllerTests;

public class MembersControllerGetMembersTests
{
    [Test, AutoData]
    public async Task GetMembers_ForwardsRequestToInnerApi(
        GetMembersResponse expected,
        CancellationToken cancellationToken)
    {
        const string queryString = "?page=1";
        Mock<IAanHubRestApiClient> apiClientMock = new();
        apiClientMock.Setup(m => m.GetMembers(queryString, cancellationToken)).ReturnsAsync(expected);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new(queryString);
        MembersController sut = new(apiClientMock.Object, Mock.Of<IMediator>())
        {
            ControllerContext = new ControllerContext() { HttpContext = httpContext }
        };

        var actual = await sut.GetMembers(cancellationToken);

        apiClientMock.Verify(m => m.GetMembers(queryString, cancellationToken));
        actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
