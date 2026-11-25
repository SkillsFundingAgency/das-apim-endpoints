using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.AODP.Shared.UnitTests.Helpers;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetApplicationExportDataQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationExportDataQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();

            _handler = _fixture.Create<GetApplicationExportDataQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_And_Data_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationExportDataQuery>();
            var response = _fixture.Create<GetApplicationExportDataQueryResponse>();

            _apiClientMock
                .Setup(x => x.Get<GetApplicationExportDataQueryResponse>(
                    It.IsAny<GetApplicationExportDetailsApiRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x =>
                x.Get<GetApplicationExportDataQueryResponse>(
                    It.IsAny<GetApplicationExportDetailsApiRequest>()),
                Times.Once);

            _apiClientMock.Verify(x =>
                x.Get<GetApplicationExportDataQueryResponse>(
                    It.Is<GetApplicationExportDetailsApiRequest>(r =>
                        r.ApplicationReviewId == query.ApplicationReviewId)),
                Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_If_Api_Throws_Exception_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationExportDataQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock
                .Setup(x => x.Get<GetApplicationExportDataQueryResponse>(
                    It.IsAny<GetApplicationExportDetailsApiRequest>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}