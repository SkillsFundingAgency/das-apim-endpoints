using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.GetRelationshipsControllerTests;

public class GetRelationshipsTests
{
    [Test, AutoData]
    public async Task GetRelationships_InvokesInnerApi_WithQueryString(long ukprn, CancellationToken cancellationToken)
    {
        const string queryString = "?pageNumber=1";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new(queryString);

        Mock<IProviderRelationshipsApiRestClient> clientMock = new();

        RelationshipsController sut = new(Mock.Of<IMediator>(), clientMock.Object, Mock.Of<ILogger<RelationshipsController>>())
        {
            ControllerContext = new ControllerContext() { HttpContext = httpContext }
        };

        await sut.GetRelationships(ukprn, new(), cancellationToken);

        clientMock.Verify(c => c.GetProviderRelationships(ukprn, queryString, cancellationToken), Times.Once);
    }

    [Test, AutoData]
    public async Task GetRelationships_ReturnsOkResponse(long ukprn, GetProviderRelationshipsResponse response, CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiRestClient> clientMock = new();
        clientMock.Setup(c => c.GetProviderRelationships(ukprn, It.IsAny<string>(), cancellationToken)).ReturnsAsync(response);

        RelationshipsController sut = new(Mock.Of<IMediator>(), clientMock.Object, Mock.Of<ILogger<RelationshipsController>>())
        {
            ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() }
        };

        var result = await sut.GetRelationships(ukprn, new(), cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(response);
    }
}
