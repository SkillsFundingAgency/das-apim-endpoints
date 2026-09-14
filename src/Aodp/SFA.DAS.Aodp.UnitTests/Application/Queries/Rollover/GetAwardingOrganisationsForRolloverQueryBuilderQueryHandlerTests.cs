using System.Net;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetAwardingOrganisationsForRolloverQueryBuilderQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetAwardingOrganisationsForRolloverQueryBuilderQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetAwardingOrganisationsForRolloverQueryBuilderQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsAwardingOrganisations_ShouldForwardFiltersAndReturnSuccessfulResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderAwardingOrganisationsRequest([3], [1], ["1.2"]);
        var apiResponse = new GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse
        {
            AwardingOrganisations =
            [
                new RolloverQueryBuilderAwardingOrganisation
                {
                    Id = Guid.NewGuid(),
                    NameLegal = "Awarding organisation"
                }
            ]
        };

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse>(
                It.Is<GetAwardingOrganisationsForRolloverQueryBuilderApiRequest>(request => request.Request == filters)))
            .ReturnsAsync(new ApiResponse<GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse>(
                apiResponse, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await _handler.Handle(
            new GetAwardingOrganisationsForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.SameAs(apiResponse));
            _mockApiClient.Verify(x => x.PostWithResponseCode<GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse>(
                It.Is<GetAwardingOrganisationsForRolloverQueryBuilderApiRequest>(request => request.Request == filters)), Times.Once);
        });
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
    {
        // Arrange
        var filters = new RolloverQueryBuilderAwardingOrganisationsRequest([3], [1], ["1.2"]);

        _mockApiClient
            .Setup(x => x.PostWithResponseCode<GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetAwardingOrganisationsForRolloverQueryBuilderApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("Unable to get awarding organisations"));

        // Act
        var result = await _handler.Handle(
            new GetAwardingOrganisationsForRolloverQueryBuilderQuery(filters), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Unable to get awarding organisations"));
        });
    }
}
