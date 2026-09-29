using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Qaa;
using SFA.DAS.Aodp.Application.Queries.Qaa;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Qaa;

[TestFixture]
public class QaaControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<QaaController>> _mockLogger;
    private Mock<IMediator> _mockMediator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockLogger = _fixture.Freeze<Mock<ILogger<QaaController>>>();
        _mockMediator = _fixture.Freeze<Mock<IMediator>>();
    }

    [Test]
    public async Task GetDownloadSummary_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var expected = new GetQaaDownloadSummaryQueryResponse
        {
            NewQualificationsCount = 1,
            ExtendedQualificationsCount = 2,
            DiscontinuedQualificationsCount = 3
        };

        var mediatorResponse = new BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>
        {
            Success = true,
            Value = expected
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetQaaDownloadSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new QaaController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.GetDownloadSummary();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ok.Value, Is.InstanceOf<GetQaaDownloadSummaryQueryResponse>());

        var returned = (GetQaaDownloadSummaryQueryResponse)ok.Value!;
        Assert.That(returned.NewQualificationsCount, Is.EqualTo(1));
        Assert.That(returned.ExtendedQualificationsCount, Is.EqualTo(2));
        Assert.That(returned.DiscontinuedQualificationsCount, Is.EqualTo(3));
    }

    [Test]
    public async Task GetDownloadSummary_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var mediatorResponse = new BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetQaaDownloadSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new QaaController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.GetDownloadSummary();

        // Assert
        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)result;
        Assert.That(status.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task Download_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var expected = new GetQaaQualificationsExportQueryResponse
        {
            FileContent = new byte[] { 1, 2, 3 },
            FileName = "export.csv",
            ContentType = "text/csv"
        };

        var mediatorResponse = new BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>
        {
            Success = true,
            Value = expected
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetQaaQualificationsExportQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new QaaController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.Download("tester");

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ok.Value, Is.InstanceOf<GetQaaQualificationsExportQueryResponse>());

        var returned = (GetQaaQualificationsExportQueryResponse)ok.Value!;
        Assert.That(returned.FileName, Is.EqualTo("export.csv"));
        Assert.That(returned.ContentType, Is.EqualTo("text/csv"));
        Assert.That(returned.FileContent, Is.EqualTo(new byte[] { 1, 2, 3 }));

        _mockMediator.Verify(
            m => m.Send(
                It.Is<GetQaaQualificationsExportQuery>(q => q.CurrentUsername == "tester"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Download_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var mediatorResponse = new BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetQaaQualificationsExportQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new QaaController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.Download("tester");

        // Assert
        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)result;
        Assert.That(status.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }
}
