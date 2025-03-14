using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.FormBuilder.Questions
{
    [TestFixture]
    public class GetQuestionByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetQuestionByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetQuestionByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Question_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQuestionByIdQuery>();
            //var response = _fixture.Create<GetQuestionByIdQueryResponse>();
            var response = new GetQuestionByIdQueryResponse()
            {
                Id = Guid.NewGuid(),
                PageId = Guid.NewGuid(),
                Title = "a",
                Key = Guid.NewGuid(),
                Hint = "b",
                HelperHTML = "c",
                Order = 1,
                Required = false,
                Type = "d",
                Editable = true
            };

            _apiClientMock.Setup(x => x.Get<GetQuestionByIdQueryResponse>(It.IsAny<GetQuestionByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQuestionByIdQueryResponse>(It.IsAny<GetQuestionByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetQuestionByIdQueryResponse>(It.Is<GetQuestionByIdApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQuestionByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetQuestionByIdQueryResponse>(It.IsAny<GetQuestionByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
