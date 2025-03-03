using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.GetRelationshipsControllerTests;

public class GetRelationshipsTests
{
    [Test, AutoData]
    public async Task GetRelationships_InvokesInnerApi_WithQueryString(long ukprn, CancellationToken cancellationToken)
    {
        var mediator = new Mock<IMediator>();

        RelationshipsController sut = new(mediator.Object)
        {
            ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() }
        };

        await sut.GetRelationships(ukprn, new(), cancellationToken);

        mediator.Verify(c => c.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken), Times.Once);
    }

    [Test, AutoData]
    public async Task GetRelationships_ReturnsOkResponse(long ukprn, GetProviderRelationshipsResponse response, CancellationToken cancellationToken)
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken)).ReturnsAsync(response);

        RelationshipsController sut = new(mediator.Object)
        {
            ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() }
        };

        var result = await sut.GetRelationships(ukprn, new(), cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(response);
    }
}
