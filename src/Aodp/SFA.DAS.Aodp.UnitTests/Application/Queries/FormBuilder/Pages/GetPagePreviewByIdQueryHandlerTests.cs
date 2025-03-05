using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Pages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.FormBuilder.Pages
{
    [TestFixture]
    public class GetPagePreviewByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetPagePreviewByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetPagePreviewByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_PagePreview_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetPagePreviewByIdQuery>();
            var response = _fixture.Create<GetPagePreviewByIdQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetPagePreviewByIdQueryResponse>(It.IsAny<GetPagePreviewByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetPagePreviewByIdQueryResponse>(It.IsAny<GetPagePreviewByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetPagePreviewByIdQueryResponse>(It.Is<GetPagePreviewByIdApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetPagePreviewByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetPagePreviewByIdQueryResponse>(It.IsAny<GetPagePreviewByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
