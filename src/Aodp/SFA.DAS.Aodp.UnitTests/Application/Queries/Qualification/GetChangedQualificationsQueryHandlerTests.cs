using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications;


[TestFixture]
public class GetChangedQualificationsQueryHandlerTests
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
    public async Task Then_The_Api_Is_Called_With_The_Request_And_ChangedQualificationsData_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsQuery>();
        var response = _fixture.Create<GetChangedQualificationsApiResponse>();
        response.Data = _fixture.CreateMany<ChangedQualification>(2).ToList();

        _apiClientMock.Setup(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()))
                      .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Value.Data.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsQuery>();
        var baseResponse = new GetChangedQualificationsApiResponse
        {
            Data = null
        };

        _apiClientMock.Setup(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()))
                      .ReturnsAsync(baseResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsQuery>();
        var exceptionMessage = "An error occurred";
        _apiClientMock.Setup(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()))
                      .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetChangedQualificationsApiResponse>(It.IsAny<GetChangedQualificationsApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
    }
}
