using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;
using Azure.Core;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetApplicationReviewSharingStatusByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationReviewSharingStatusByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationReviewSharingStatusByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_ApplicationFormData_By_Id_Is_Returned()
        {
            Guid applicationReviewId = Guid.NewGuid();

            // Arrange
            var query = new GetApplicationReviewSharingStatusByIdQuery(applicationReviewId);

            var body = _fixture.Build<GetApplicationReviewSharingStatusByIdQueryResponse>()
                                .Create();
            var apiResponse = new ApiResponse<GetApplicationReviewSharingStatusByIdQueryResponse>(body, System.Net.HttpStatusCode.OK, "");

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetApplicationReviewSharingStatusByIdQueryResponse>(It.IsAny<GetApplicationReviewSharingStatusByIdApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetApplicationReviewSharingStatusByIdQueryResponse>(It.IsAny<GetApplicationReviewSharingStatusByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.GetWithResponseCode<GetApplicationReviewSharingStatusByIdQueryResponse>(It.Is<GetApplicationReviewSharingStatusByIdApiRequest>(r => r.ApplicationReviewId == query.ApplicationReviewId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse.Body));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationReviewSharingStatusByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetApplicationReviewSharingStatusByIdQueryResponse>(It.IsAny<GetApplicationReviewSharingStatusByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
