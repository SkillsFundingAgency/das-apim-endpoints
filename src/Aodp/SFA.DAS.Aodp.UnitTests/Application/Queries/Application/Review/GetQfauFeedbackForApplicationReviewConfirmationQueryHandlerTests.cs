using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using MediatR;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Review;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetQfauFeedbackForApplicationReviewConfirmationQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private GetQfauFeedbackForApplicationReviewConfirmationQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _handler = _fixture.Create<GetQfauFeedbackForApplicationReviewConfirmationQueryHandler>();
        }

        [Test]
        public async Task Then_The_Mediatr_Is_Called_With_The_Requests_And_Feedback_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQfauFeedbackForApplicationReviewConfirmationQuery>();
            var feedbackResponse = _fixture.Create<GetFeedbackForApplicationReviewByIdQueryResponse>();
            var qualResponse = _fixture.Create<GetRelatedQualificationForApplicationQueryResponse>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetFeedbackForApplicationReviewByIdQuery>(), default))
                          .ReturnsAsync(new BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>() { Value = feedbackResponse, Success = true });


            _mediatorMock.Setup(x => x.Send(It.IsAny<GetRelatedQualificationForApplicationQuery>(), default))
                          .ReturnsAsync(new BaseMediatrResponse<GetRelatedQualificationForApplicationQueryResponse>() { Value = qualResponse, Success = true });
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetFeedbackForApplicationReviewByIdQuery>(), default), Times.Once);
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetRelatedQualificationForApplicationQuery>(), default), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        public async Task Then_The_Mediatr_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQfauFeedbackForApplicationReviewConfirmationQuery>();
            var feedbackResponse = _fixture.Create<GetFeedbackForApplicationReviewByIdQueryResponse>();
            var qualResponse = _fixture.Create<GetRelatedQualificationForApplicationQueryResponse>();

            _mediatorMock.Setup(x => x.Send(It.IsAny<GetFeedbackForApplicationReviewByIdQuery>(), default))
                          .ReturnsAsync(new BaseMediatrResponse<GetFeedbackForApplicationReviewByIdQueryResponse>() { Value = feedbackResponse, Success = false });


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        public class DateOnlySpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (request is Type type && type == typeof(DateOnly))
                {
                    return DateOnly.FromDateTime(DateTime.Now);
                }

                return new NoSpecimen();
            }
        }
    }
}
