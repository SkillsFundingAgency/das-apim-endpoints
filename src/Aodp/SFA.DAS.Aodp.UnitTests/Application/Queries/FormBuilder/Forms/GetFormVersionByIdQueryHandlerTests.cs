using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.FormBuilder.Forms
{
    [TestFixture]
    public class GetFormVersionByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetFormVersionByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new GetFormVersionByIdQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_FormVersion_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetFormVersionByIdQuery>();
            var response = _fixture.Create<GetFormVersionByIdQueryResponse>();

            ApiResponse<GetFormVersionByIdQueryResponse> apiResponse = new(response, System.Net.HttpStatusCode.OK, string.Empty);

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFormVersionByIdQueryResponse>(It.IsAny<GetFormVersionByIdApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFormVersionByIdQueryResponse>(It.IsAny<GetFormVersionByIdApiRequest>()), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetFormVersionByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFormVersionByIdQueryResponse>(It.IsAny<GetFormVersionByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
        }
    }
}
