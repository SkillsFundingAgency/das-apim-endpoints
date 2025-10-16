using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications;


[TestFixture]
public class GetQualificationOutputFileQueryHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private GetChangedQualificationsQueryHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<GetChangedQualificationsQueryHandler>();
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_OutputFile_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetQualificationOutputFileQuery>();

        var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileResponse>>()
            .With(r => r.Success, true)
            .With(r => r.Value, new GetQualificationOutputFileResponse
            {
                FileName = "qualifications_export.zip",
                ZipFileContent = new byte[] { 1, 2, 3 },
                ContentType = "application/zip"
            })
            .Create();

        _apiClientMock
            .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()))
            .ReturnsAsync(apiResponse);

        var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(
            x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()),
            Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.FileName, Is.EqualTo(apiResponse.Value.FileName));
            Assert.That(result.Value.ZipFileContent, Is.Not.Null.And.Not.Empty);
            Assert.That(result.Value.ContentType, Is.EqualTo("application/zip"));
        });
    }


    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned_When_Success_Is_False()
    {
        // Arrange
        var query = _fixture.Create<GetQualificationOutputFileQuery>();

        var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileResponse>>()
            .With(r => r.Success, false)
            .With(r => r.ErrorMessage, "Something went wrong")
            .With(r => r.Value, (GetQualificationOutputFileResponse?)null)
            .Create();

        _apiClientMock
            .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()))
            .ReturnsAsync(apiResponse);

        var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(
            x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()),
            Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Something went wrong"));
        });
    }


    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetQualificationOutputFileQuery>();
        var exceptionMessage = "Something exploded in the API";

        _apiClientMock
            .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(
            x => x.Get<BaseMediatrResponse<GetQualificationOutputFileResponse>>(It.IsAny<GetQualificationOutputFileApiRequest>()),
            Times.Once);

        Assert.Multiple(() => 
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        });
    }


}
