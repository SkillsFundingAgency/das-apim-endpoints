using System.Net;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetTypesForRolloverQueryBuilderQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetTypesForRolloverQueryBuilderQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetTypesForRolloverQueryBuilderQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsTypes_ShouldForwardLevelFiltersAndReturnSuccessfulResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderTypesRequest([3, 4]);
        var apiResponse = new GetTypesForRolloverQueryBuilderQueryResponse
        {
            Types = [new RolloverType { Id = 1, Name = "Technical" }]
        };

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetTypesForRolloverQueryBuilderQueryResponse>(
                It.Is<GetTypesForRolloverQueryBuilderApiRequest>(request => request.Request == filters)))
            .ReturnsAsync(new ApiResponse<GetTypesForRolloverQueryBuilderQueryResponse>(
                apiResponse, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await _handler.Handle(new GetTypesForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.SameAs(apiResponse));
            _mockApiClient.Verify(x => x.PostWithResponseCode<GetTypesForRolloverQueryBuilderQueryResponse>(
                It.Is<GetTypesForRolloverQueryBuilderApiRequest>(request => request.Request == filters)), Times.Once);
        });
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderTypesRequest([3]);

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetTypesForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetTypesForRolloverQueryBuilderApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("Unable to get types"));

        // Act
        var result = await _handler.Handle(new GetTypesForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Unable to get types"));
        });
    }
}
