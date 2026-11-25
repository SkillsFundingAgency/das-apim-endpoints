using System.Net;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetSectorSubjectAreasForRolloverQueryBuilderQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetSectorSubjectAreasForRolloverQueryBuilderQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetSectorSubjectAreasForRolloverQueryBuilderQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsSectorSubjectAreas_ShouldForwardFiltersAndReturnSuccessfulResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderSectorSubjectAreaRequest([3, 4], [1, 2]);
        var apiResponse = new GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse
        {
            SectorSubjectAreas = [new RolloverSectorSubjectArea { Id = "1.2", Name = "Engineering" }]
        };

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>(
                It.Is<GetSectorSubjectAreaForRolloverQueryBuilderApiRequest>(request => request.Request == filters)))
            .ReturnsAsync(new ApiResponse<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>(
                apiResponse, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await _handler.Handle(
            new GetSectorSubjectAreaForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.SameAs(apiResponse));
            _mockApiClient.Verify(x => x.PostWithResponseCode<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>(
                It.Is<GetSectorSubjectAreaForRolloverQueryBuilderApiRequest>(request => request.Request == filters)), Times.Once);
        });
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderSectorSubjectAreaRequest([3], [1]);

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetSectorSubjectAreaForRolloverQueryBuilderApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("Unable to get sector subject areas"));

        // Act
        var result = await _handler.Handle(
            new GetSectorSubjectAreaForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Unable to get sector subject areas"));
        });
    }
}
