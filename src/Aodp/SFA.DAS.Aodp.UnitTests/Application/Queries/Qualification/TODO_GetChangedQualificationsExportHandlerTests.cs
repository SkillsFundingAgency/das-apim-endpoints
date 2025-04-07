using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Azure.Core;
using MediatR;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Form;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualification;

[TestFixture]
public class GetChangedQualificationExportHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private GetChangedQualificationsExportHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<GetChangedQualificationsExportHandler>();
    }


    public class DateOnlySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DateOnly))
            {
                return new DateOnly(2023, 1, 1); //a valid default date here
            }

            return new NoSpecimen();
        }
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_ChangedQualificationsData_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsExportQuery>();
        var response = _fixture.Create<BaseMediatrResponse<GetQualificationsExportResponse>>();


        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()))
                      .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsExportQuery>();
        var baseResponse = _fixture.Create<BaseMediatrResponse<GetQualificationsExportResponse>>();
        baseResponse.Value = null;

        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()))
                      .ReturnsAsync(baseResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
    {
        // Arrange
        var query = _fixture.Create<GetChangedQualificationsExportQuery>();
        var exceptionMessage = "An error occurred";
        _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()))
                      .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetQualificationsExportResponse>>(It.IsAny<GetChangedQualificationCsvExportApiRequest>()), Times.Once);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
    }
}
