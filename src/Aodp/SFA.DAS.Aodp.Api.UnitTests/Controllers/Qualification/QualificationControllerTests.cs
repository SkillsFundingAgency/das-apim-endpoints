using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.AODP.Api.Controllers.Qualification;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Qualification
{
    [TestFixture]
    public class QualificationsControllerTests
    {
        private IFixture _fixture;
        private Mock<ILogger<QualificationsController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private QualificationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _loggerMock = _fixture.Freeze<Mock<ILogger<QualificationsController>>>();
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetQualifications_ReturnsOkResult_WithListOfNewQualifications()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetNewQualificationsQueryResponse>>();
            queryResponse.Success = true;
            queryResponse.Value.NewQualifications = _fixture.CreateMany<NewQualification>(2).ToList();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNewQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetQualifications("new");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<BaseMediatrResponse<GetNewQualificationsQueryResponse>>());
            var model = (BaseMediatrResponse<GetNewQualificationsQueryResponse>)okResult.Value;
            Assert.That(model.Value.NewQualifications.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetQualifications_ReturnsNotFound_WhenQueryFails()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetNewQualificationsQueryResponse>>();
            queryResponse.Success = false;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNewQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetQualifications("new");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)result;
            var notFoundValue = notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value, null);
            Assert.That(notFoundValue, Is.EqualTo("No new qualifications found"));
        }

        [Test]
        public async Task GetQualifications_ReturnsBadRequest_WhenStatusIsEmpty()
        {
            // Act
            var result = await _controller.GetQualifications(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            var badRequestValue = badRequestResult.Value?.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
            Assert.That(badRequestValue, Is.EqualTo("Qualification status cannot be empty."));
        }

        [Test]
        public async Task GetQualificationDetails_ReturnsOkResult_WithQualificationDetails()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>();
            queryResponse.Success = true;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetQualificationDetailsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetQualificationDetails("Ref123");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>());
            var model = (BaseMediatrResponse<GetQualificationDetailsQueryResponse>)okResult.Value;
            Assert.That(model.Value.Id, Is.EqualTo(queryResponse.Value.Id));
        }

        [Test]
        public async Task GetQualificationDetails_ReturnsNotFound_WhenQueryFails()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>();
            queryResponse.Success = false;
            queryResponse.ErrorMessage = "No details found for qualification reference: Ref123";

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetQualificationDetailsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await _controller.GetQualificationDetails("Ref123");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetQualificationDetails_ReturnsBadRequest_WhenQualificationReferenceIsEmpty()
        {
            // Act
            var result = await _controller.GetQualificationDetails(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            var badRequestValue = badRequestResult.Value?.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
            Assert.That(badRequestValue, Is.EqualTo("Qualification reference cannot be empty"));
        }
    }
}
