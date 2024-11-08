using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Requests;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.GetRelationshipsControllerTests;

public class GetRelationshipsTests
{
    [Test, AutoData]
    public async Task GetRelationships_InvokesInnerApi_WithQueryString(long ukprn, CancellationToken cancellationToken)
    {
        var request = new GetProviderRelationshipsRequest();

        var httpContext = new DefaultHttpContext();

        var mediator = new Mock<IMediator>();

        RelationshipsController sut = new(mediator.Object, Mock.Of<IProviderRelationshipsApiRestClient>(), Mock.Of<ILogger<RelationshipsController>>())
        {
            ControllerContext = new ControllerContext() { HttpContext = httpContext }
        };

        var query = new GetRelationshipsQuery(ukprn, request);

        await sut.GetRelationships(ukprn, request, cancellationToken);

        mediator.Verify(c => c.Send(query, cancellationToken), Times.Once);
    }

    [Test, AutoData]
    public async Task GetRelationships_ReturnsOkResponse(long ukprn, Response<GetProviderRelationshipsResponse> response, CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiRestClient> clientMock = new();
        clientMock.Setup(c => c.GetProviderRelationships(ukprn, It.IsAny<GetProviderRelationshipsRequest>(), cancellationToken)).ReturnsAsync(response);

        RelationshipsController sut = new(Mock.Of<IMediator>(), clientMock.Object, Mock.Of<ILogger<RelationshipsController>>())
        {
            ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() }
        };

        var result = await sut.GetRelationships(ukprn, new(), cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(response);
    }
}
