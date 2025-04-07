using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.FundingOffer;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.AODP.Application.Queries.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualification;

[TestFixture]
public class GetQualificationVersionHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private GetQualificationVersionHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<GetQualificationVersionHandler>();
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_ChangedQualificationsData_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetQualificationVersionQuery>();

        var body = _fixture.Build<GetQualificationDetailsQueryResponse>()
            .Create();
        var apiResponse = new ApiResponse<GetQualificationDetailsQueryResponse>(body, System.Net.HttpStatusCode.OK, "");

        _apiClientMock.Setup(x => x.GetWithResponseCode<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationVersionApiRequest>()))
                      .ReturnsAsync(apiResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.GetWithResponseCode<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationVersionApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetQualificationVersionQuery>();
        var exceptionMessage = "An error occurred";
        _apiClientMock.Setup(x => x.GetWithResponseCode<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationVersionApiRequest>()))
                      .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.GetWithResponseCode<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationVersionApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
    }
}
