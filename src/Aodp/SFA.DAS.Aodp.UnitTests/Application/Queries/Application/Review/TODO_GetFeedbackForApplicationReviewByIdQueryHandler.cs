using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using static SFA.DAS.Aodp.Application.Queries.Application.Review.GetApplicationForReviewByIdQueryResponse;
using SFA.DAS.SharedOuterApi.Models;
using AutoFixture.Kernel;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetFeedbackForApplicationReviewByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetFeedbackForApplicationReviewByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetFeedbackForApplicationReviewByIdQueryHandler>();
        }

        public class DateOnlySpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (request is Type type && type == typeof(DateOnly))
                {
                    return new DateOnly(2023, 1, 1); // a valid date
                }

                return new NoSpecimen();
            }
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_ApplicationFormData_By_Id_Is_Returned()
        {
            // Arrange
            Guid applicationReviewId = Guid.NewGuid();

            string userType = " ";

            var query = new GetFeedbackForApplicationReviewByIdQuery(applicationReviewId, userType);

            var body = _fixture.Build<GetFeedbackForApplicationReviewByIdQueryResponse>()
                    .Create();
            var apiResponse = new ApiResponse<GetFeedbackForApplicationReviewByIdQueryResponse>(body, System.Net.HttpStatusCode.OK, "");

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFeedbackForApplicationReviewByIdQueryResponse>(It.IsAny<GetFeedbackForApplicationReviewByIdApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFeedbackForApplicationReviewByIdQueryResponse>(It.IsAny<GetFeedbackForApplicationReviewByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFeedbackForApplicationReviewByIdQueryResponse>(It.Is<GetFeedbackForApplicationReviewByIdApiRequest>(r => r.ApplicationReviewId == query.ApplicationReviewId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse.Body));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetFeedbackForApplicationReviewByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetFeedbackForApplicationReviewByIdQueryResponse>(It.IsAny<GetFeedbackForApplicationReviewByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
