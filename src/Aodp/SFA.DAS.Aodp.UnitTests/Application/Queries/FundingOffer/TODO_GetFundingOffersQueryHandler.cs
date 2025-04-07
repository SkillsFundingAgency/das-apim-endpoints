using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;
using SFA.DAS.Aodp.Application.Queries.FundingOffer;
using Microsoft.Extensions.Azure;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetFundingOffersQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetFundingOffersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetFundingOffersQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_ApplicationFormData_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetFundingOffersQuery>();

            var body = _fixture.Build<GetFundingOffersQueryResponse>()
                .Create();
            var apiResponse = new ApiResponse<GetFundingOffersQueryResponse>(body, System.Net.HttpStatusCode.OK, "");

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFundingOffersQueryResponse>(It.IsAny<GetFundingOffersApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFundingOffersQueryResponse>(It.IsAny<GetFundingOffersApiRequest>()), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse.Body));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetFundingOffersQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFundingOffersQueryResponse>(It.IsAny<GetFundingOffersApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
