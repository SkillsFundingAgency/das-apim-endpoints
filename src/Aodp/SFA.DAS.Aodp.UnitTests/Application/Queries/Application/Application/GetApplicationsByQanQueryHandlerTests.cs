using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Application;

[TestFixture]
public class GetApplicationsByQanQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock = null!;
    private GetApplicationsByQanQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _apiClientMock = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetApplicationsByQanQueryHandler(_apiClientMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessAndMapsValue_WhenApiReturnsResponse()
    {
        // Arrange
        var qan = "QAN-123";
        var apiResponse = new GetApplicationsByQanQueryResponse
        {
            Applications = new System.Collections.Generic.List<GetApplicationsByQanQueryResponse.Application>
                {
                    new GetApplicationsByQanQueryResponse.Application { Id = Guid.NewGuid(), Name = "App 1" }
                }
        };

        _apiClientMock
            .Setup(c => c.Get<GetApplicationsByQanQueryResponse>(It.IsAny<GetApplicationsByQanApiRequest>()))
            .ReturnsAsync(apiResponse);

        var request = new GetApplicationsByQanQuery(qan);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True, "Expected Success = true for successful API response");
        Assert.That(result.Value, Is.SameAs(apiResponse), "Expected the handler to return the same instance the api client returned");
        _apiClientMock.Verify(c => c.Get<GetApplicationsByQanQueryResponse>(It.Is<GetApplicationsByQanApiRequest>(r => r.Qan == qan)), Times.Once);
    }

    [Test]
    public async Task Handle_ReturnsSuccess_WhenApiReturnsNull()
    {
        // Arrange
        var qan = "QAN-456";

        _apiClientMock
            .Setup(c => c.Get<GetApplicationsByQanQueryResponse>(It.IsAny<GetApplicationsByQanApiRequest>()))
            .ReturnsAsync((GetApplicationsByQanQueryResponse?)null);

        var request = new GetApplicationsByQanQuery(qan);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True, "Handler should set Success = true even when API returns null (no exception thrown)");
        Assert.That(result.Value, Is.Null, "Value should be null when API returns null");
        _apiClientMock.Verify(c => c.Get<GetApplicationsByQanQueryResponse>(It.Is<GetApplicationsByQanApiRequest>(r => r.Qan == qan)), Times.Once);
    }

    [Test]
    public async Task Handle_ReturnsFailureAndErrorMessage_WhenApiThrows()
    {
        // Arrange
        var qan = "QAN-999";
        var ex = new InvalidOperationException("Boom!");

        _apiClientMock
            .Setup(c => c.Get<GetApplicationsByQanQueryResponse>(It.IsAny<GetApplicationsByQanApiRequest>()))
            .ThrowsAsync(ex);

        var request = new GetApplicationsByQanQuery(qan);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.False, "Expected Success = false when api client throws");
        Assert.That(result.ErrorMessage, Is.EqualTo("Boom!"), "Expected the exception message to be copied to ErrorMessage");
        _apiClientMock.Verify(c => c.Get<GetApplicationsByQanQueryResponse>(It.Is<GetApplicationsByQanApiRequest>(r => r.Qan == qan)), Times.Once);
    }
}
