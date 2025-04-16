using AutoFixture;
using SFA.DAS.AODP.Application.Queries.OutputFile;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;

namespace SFA.DAS.AODP.Application.UnitTests.Queries.OutputFile;

[TestFixture]
public class WhenHandlingGetPreviousOutputFilesQuery
{
    private readonly Fixture _fixture = new();
    private readonly GetPreviousOutputFilesQueryHandler _handler;
    private readonly Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock = new();

    public WhenHandlingGetPreviousOutputFilesQuery()
    {
        _handler = new(_apiClientMock.Object);
    }

    [Test]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected()
    {
        // Arrange
        var expectedResponse = _fixture.Create<GetPreviousOutputFilesQueryResponse>();

        var request = _fixture.Create<GetPreviousOutputFilesQuery>();
        _apiClientMock
            .Setup(a => a.Get<GetPreviousOutputFilesQueryResponse>(It.IsAny<GetPreviousOutputFilesApiRequest>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _apiClientMock
              .Verify(a => a.Get<GetPreviousOutputFilesQueryResponse>(It.IsAny<GetPreviousOutputFilesApiRequest>()));

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Success, Is.True);
        Assert.That(response.Value, Is.Not.Null);
        Assert.That(expectedResponse, Is.EqualTo(response.Value));
    }

    [Test]
    public async Task And_Api_Errors_Then_The_FailCommandResult_Is_Returned()
    {
        // Arrange
        var expectedException = _fixture.Create<Exception>();
        var request = _fixture.Create<GetPreviousOutputFilesQuery>();
        _apiClientMock
            .Setup(a => a.Get<GetPreviousOutputFilesQueryResponse>(It.IsAny<GetPreviousOutputFilesApiRequest>()))
            .ThrowsAsync(expectedException);


        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Success, Is.False);
        Assert.That(response.ErrorMessage, Is.Not.Empty);
        Assert.That(expectedException.Message, Is.EqualTo(response.ErrorMessage));
    }
}
