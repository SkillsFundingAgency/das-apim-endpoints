using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Application
{
    [TestFixture]
    public class GetRelatedQualificationForApplicationQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetRelatedQualificationForApplicationQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetRelatedQualificationForApplicationQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Qualification_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRelatedQualificationForApplicationQuery>();
            var response = _fixture.Create<GetRelatedQualificationForApplicationQueryResponse>();
            var apiResult = new ApiResponse<GetRelatedQualificationForApplicationQueryResponse>(
             response,
             System.Net.HttpStatusCode.OK,
             string.Empty,
             new Dictionary<string, IEnumerable<string>>()
         );
            _apiClientMock.Setup(x => x.GetWithResponseCode<GetRelatedQualificationForApplicationQueryResponse>(It.IsAny<GetRelatedQualificationForApplicationApiRequest>()))
                          .ReturnsAsync(apiResult);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetRelatedQualificationForApplicationQueryResponse>(It.IsAny<GetRelatedQualificationForApplicationApiRequest>()), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRelatedQualificationForApplicationQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetRelatedQualificationForApplicationQueryResponse>(It.IsAny<GetRelatedQualificationForApplicationApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }

}
