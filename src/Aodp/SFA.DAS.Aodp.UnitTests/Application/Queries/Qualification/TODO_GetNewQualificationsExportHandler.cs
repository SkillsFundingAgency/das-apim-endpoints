using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualification;

[TestFixture]
public class GetNewQualificationsExportHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private GetNewQualificationsExportHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<GetNewQualificationsExportHandler>();
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_ChangedQualificationsData_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetNewQualificationsExportQuery>();
        var response = _fixture.Create<BaseMediatrResponse<GetQualificationsExportResponse>>();

        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()))
                      .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetNewQualificationsExportQuery>();
        var baseResponse = new BaseMediatrResponse<GetQualificationsExportResponse>()
        {

        };
        baseResponse = null;

        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()))
                      .ReturnsAsync(baseResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetNewQualificationsExportQuery>();
        var exceptionMessage = "An error occurred";
        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()))
                      .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetNewQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
    }
}
