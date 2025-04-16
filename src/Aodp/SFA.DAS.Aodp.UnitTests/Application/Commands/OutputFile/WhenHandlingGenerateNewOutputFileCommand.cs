using AutoFixture;
using Moq;
using SFA.DAS.AODP.Application.Commands.OutputFile;
using SFA.DAS.AODP.Application.Queries.OutputFile;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Application.UnitTests.Commands.OutputFile;

[TestFixture]
public class WhenHandlingGenerateNewOutputFileCommand
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = new();
    private readonly GenerateNewOutputFileCommandHandler _handler;

    public WhenHandlingGenerateNewOutputFileCommand()
    {
        _handler = new(_apiClient.Object);
    }

    [Test]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected()
    {
        // Arrange
        var expectedResponse = _fixture
            .Build<BaseMediatrResponse<EmptyResponse>>()
            .With(w => w.Success, true)
            .Create();
        var expectedApiResponse = _fixture.Create<ApiResponse<EmptyResponse>>();

        _apiClient.Setup(a => a.PostWithResponseCode<EmptyResponse>(It.IsAny<GenerateNewOutputFileApiRequest>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(expectedApiResponse));

        var request = _fixture.Create<GenerateNewOutputFileCommand>();

        // Act
        var response = await _handler.Handle(request, default);

        // Assert
        _apiClient
            .Verify(a => a.PostWithResponseCode<EmptyResponse>(It.IsAny<GenerateNewOutputFileApiRequest>(), It.IsAny<bool>()));

        Assert.That(response.Success, Is.True);
        Assert.That(response.Value, Is.Not.Null);
    }

    [Test]
    public async Task And_Api_Errors_Then_The_FailCommandResult_Is_Returned()
    {
        // Arrange
        var expectedException = _fixture.Create<Exception>();
        var request = _fixture.Create<GenerateNewOutputFileCommand>();
        _apiClient
            .Setup(a => a.PostWithResponseCode<EmptyResponse>(It.IsAny<GenerateNewOutputFileApiRequest>(), It.IsAny<bool>()))
            .ThrowsAsync(expectedException);

        // Act
        var response = await _handler.Handle(request, default);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Success, Is.False);
        Assert.That(response.ErrorMessage, Is.Not.Empty);
        Assert.That(expectedException.Message, Is.EqualTo(response.ErrorMessage));
    }
}
