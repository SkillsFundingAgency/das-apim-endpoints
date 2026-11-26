using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.AODP.Shared.UnitTests.Helpers;

namespace SFA.DAS.Aodp.Application.UnitTests.Queries.Qualifications
{
    [TestFixture]
    public class GetFeedbackForQualificationFundingByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetFeedbackForQualificationFundingByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new GetFeedbackForQualificationFundingByIdQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccessResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var query = _fixture.Create<GetFeedbackForQualificationFundingByIdQuery>();
            var apiResponse = _fixture.Create<GetFeedbackForQualificationFundingByIdQueryResponse>();
            var apiResult = new ApiResponse<GetFeedbackForQualificationFundingByIdQueryResponse>(
                apiResponse,
                System.Net.HttpStatusCode.OK,
                string.Empty,
                new Dictionary<string, IEnumerable<string>>());


            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFeedbackForQualificationFundingByIdQueryResponse>(It.IsAny<GetFeedbackForQualificationFundingByIdApiRequest>()))
                          .ReturnsAsync(apiResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse));
        }

        [Test]
        public async Task Handle_ReturnsErrorResponse_WhenApiCallFails()
        {
            // Arrange
            var query = _fixture.Create<GetFeedbackForQualificationFundingByIdQuery>();
            var exceptionMessage = "API call failed";

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFeedbackForQualificationFundingByIdQueryResponse>(It.IsAny<GetFeedbackForQualificationFundingByIdApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }

}

