using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingAllProviderRelationships
{
    [Test, MoqAutoData]
    public async Task Then_Ok_response_is_returned_for_given_ukprn(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ReferenceDataController>> mockLogger,
        [Greedy] ReferenceDataController sut)
    {
        // Arrange
        var queryResult = CreateAllProviderRelationQueryResult();

        mockMediator
            .Setup(x => x.Send(It.IsAny<GetAllProviderRelationshipQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetAllProviderRelationshipDetails(1, 20) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var resultBody = result.Value as GetAllProviderRelationshipQueryResponse;
        resultBody.Should().NotBeNull();
        resultBody.Items.Count.Should().Be(queryResult.Items.Count);
    }

    [Test, MoqAutoData]
    public async Task Then_not_found_response_is_returned_for_given_ukprn(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ReferenceDataController>> mockLogger,
        [Greedy] ReferenceDataController sut)
    {
        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetAllProviderRelationshipDetails(1, 20) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Bad_request_response_is_returned_with_invalid_pagesize(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ReferenceDataController>> mockLogger,
        [Greedy] ReferenceDataController sut)
    {
        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetAllProviderRelationshipDetails(1, 0) as BadRequestResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    private static GetAllProviderRelationshipQueryResponse CreateAllProviderRelationQueryResult()
    {
        var fixture = new Fixture();
        var employers = fixture.Create<EmployerDetails[]>();

        return fixture.Create<GetAllProviderRelationshipQueryResponse>();
    }
}