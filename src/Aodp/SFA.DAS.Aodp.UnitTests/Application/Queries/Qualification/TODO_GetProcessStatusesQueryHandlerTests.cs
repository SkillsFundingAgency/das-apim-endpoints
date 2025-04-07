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
public class GetProcessStatusesQueryHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private GetProcessStatusesQueryHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<GetProcessStatusesQueryHandler>();
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_ChangedQualificationsData_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetProcessStatusesQuery>();
        var response = _fixture.Create<GetProcessStatusesQueryResponse>();

        _apiClientMock.Setup(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()))
                      .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetProcessStatusesQuery>();
        var baseResponse = new GetProcessStatusesQueryResponse();
        baseResponse = null;

        _apiClientMock.Setup(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()))
                      .ReturnsAsync(baseResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetProcessStatusesQuery>();
        var exceptionMessage = "An error occurred";
        _apiClientMock.Setup(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()))
                      .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<GetProcessStatusesQueryResponse>(It.IsAny<GetProcessStatusesApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
    }
}
