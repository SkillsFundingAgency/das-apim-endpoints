using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Form;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Form
{
    [TestFixture]
    public class GetApplicationFormByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationFormByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationFormByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_FormId_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationFormByIdQuery>();
            var response = _fixture.Create<GetApplicationFormByIdQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetApplicationFormByIdQueryResponse>(It.IsAny<GetApplicationFormByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetApplicationFormByIdQueryResponse>(It.IsAny<GetApplicationFormByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetApplicationFormByIdQueryResponse>(It.Is<GetApplicationFormByIdApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationFormByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetApplicationFormByIdQueryResponse>(It.IsAny<GetApplicationFormByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
