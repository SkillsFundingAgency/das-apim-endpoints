using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.Aodp.Application.Commands.Application.Qualifications;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.AODP.Api.Controllers.Qualification;
using SFA.DAS.AODP.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Qualification
{
    [TestFixture]
    public class QualificationsControllerTests
    {
        private IFixture _fixture;
        private Mock<ILogger<QualificationsController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _loggerMock = _fixture.Freeze<Mock<ILogger<QualificationsController>>>();
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        }

        [Test]
        public async Task GetQualifications_ReturnsOkResult_WithListOfNewQualifications()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetNewQualificationsQueryResponse>>();
            queryResponse.Success = true;
            queryResponse.Value.Data = _fixture.CreateMany<NewQualification>(2).ToList();
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNewQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualifications(status: "new", skip: 0, take: 10, name: "", organisation: "", qan: "",processStatusFilter: null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetNewQualificationsQueryResponse>());
            var model = (GetNewQualificationsQueryResponse)okResult.Value;
            Assert.That(model.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetQualifications_ReturnsOkResult_WithListOfChangedQualifications()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>();
            queryResponse.Success = true;
            queryResponse.Value.Data = _fixture.CreateMany<ChangedQualification>(2).ToList();
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetChangedQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualifications(status: "changed", skip: 0, take: 10, name: "", organisation: "", qan: "", processStatusFilter: null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetChangedQualificationsQueryResponse>());
            var model = (GetChangedQualificationsQueryResponse)okResult.Value;
            Assert.That(model.Data.Count, Is.EqualTo(2));
        }



        [Test]
        public async Task GetQualifications_ReturnsNotFound_WhenQueryFails()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetNewQualificationsQueryResponse>>();
            queryResponse.Success = false;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNewQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualifications(status: "new", skip: 0, take: 10, name: "", organisation: "", qan: "", processStatusFilter: null);

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result;            
        }

        [Test]
        public async Task GetChangedQualifications_ReturnsNotFound_WhenQueryFails()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>();
            queryResponse.Success = false;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetChangedQualificationsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualifications(status: "new", skip: 0, take: 10, name: "", organisation: "", qan: "", processStatusFilter: null);

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result;
        }

        [Test]
        public async Task GetQualifications_ReturnsBadRequest_WhenStatusIsEmpty()
        {
            //Arrange
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            // Act
            var result = await controller.GetQualifications(status: "", skip: 0, take: 10, name: "", organisation: "", qan: "", processStatusFilter: null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            var badRequestValue = badRequestResult.Value?.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
            Assert.That(badRequestValue, Is.EqualTo("Qualification status cannot be empty."));
        }

        [Test]
        public async Task GetChangedQualifications_ReturnsBadRequest_WhenStatusIsEmpty()
        {
            // Act
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var result = await controller.GetQualifications("", 0, 0, "", "", "", null);

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
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>();
            queryResponse.Success = true;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetQualificationDetailsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualificationDetails("Ref123");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetQualificationDetailsQueryResponse>());
            var model = (GetQualificationDetailsQueryResponse)okResult.Value;
            Assert.That(model.Id, Is.EqualTo(queryResponse.Value.Id));
        }

        [Test]
        public async Task GetQualificationDetails_ReturnsBadRequest_WhenQualificationReferenceIsEmpty()
        {
            //Arrange
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            // Act
            var result = await controller.GetQualificationDetails(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            var badRequestValue = badRequestResult.Value?.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
            Assert.That(badRequestValue, Is.EqualTo("Qualification reference cannot be empty"));
        }

        [Test]
        public async Task AddQualificationDiscussionHistory_ReturnsOkResult()
        {
            //Arrange
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var queryResponse = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
            var command = _fixture.Create<AddQualificationDiscussionHistoryCommand>();
            queryResponse.Success = true;

            _mediatorMock.Setup(m => m.Send(It.IsAny<AddQualificationDiscussionHistoryCommand>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.AddQualificationDiscussionHistory(command);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetDiscussionHistoriesForQualification_ReturnsOk()
        {
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>>();
            queryResponse.Success = true;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetDiscussionHistoriesForQualificationQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetDiscussionHistoriesForQualification("Ref123");
            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<GetDiscussionHistoriesForQualificationQueryResponse>());
            var model = (GetDiscussionHistoriesForQualificationQueryResponse)okResult.Value;
            Assert.That(queryResponse.Value.QualificationDiscussionHistories[0].Id, Is.EqualTo(model.QualificationDiscussionHistories[0].Id));
        }

        [Test]
        public async Task GetQualificationVersionsForQualificationByReference_ReturnsOkResult_WithVersions()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>>();
            queryResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetQualificationVersionsForQualificationByReferenceQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualificationVersionsForQualificationByReference("Ref123");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetQualificationVersionsForQualificationByReferenceQueryResponse>());
        }

        [Test]
        public async Task GetQualificationDetailsWithVersions_ReturnsOkResult_WithVersions()
        {
            // Arrange
            var queryResponse = _fixture.Create<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>();
            queryResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetQualificationDetailWithVersionsQuery>(), default))
                         .ReturnsAsync(queryResponse);

            // Act
            var result = await controller.GetQualificationDetailWithVersions("Ref123");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetQualificationDetailsQueryResponse>());
        }

        [Test]
        public async Task GetDiscussionHistoriesForQualification_ReturnsBadRequest_WhenQualificationReferenceIsEmpty()
        {
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            // Act
            var result = await controller.GetDiscussionHistoriesForQualification(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            var badRequestValue = badRequestResult.Value?.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
            Assert.That(badRequestValue, Is.EqualTo("Qualification reference cannot be empty"));
        }

        [Test]
        public async Task SaveQualificationFundingOffersOutcome_ReturnsOkResult()
        {
            // Arrange
            var commandResponse = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
            commandResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var command = _fixture.Create<SaveQualificationFundingOffersOutcomeCommand>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<SaveQualificationFundingOffersOutcomeCommand>(), default))
                         .ReturnsAsync(commandResponse);

            // Act
            var result = await controller.SaveQualificationFundingOffersOutcome(command, Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task SaveQualificationFundingOffers_ReturnsOkResult()
        {
            // Arrange
            var commandResponse = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
            commandResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var command = _fixture.Create<SaveQualificationFundingOffersCommand>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<SaveQualificationFundingOffersCommand>(), default))
                         .ReturnsAsync(commandResponse);

            // Act
            var result = await controller.SaveQualificationFundingOffers(command, Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task SaveFundingOfferDetails_ReturnsOkResult()
        {
            // Arrange
            var commandResponse = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
            commandResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var command = _fixture.Create<SaveQualificationFundingOffersDetailsCommand>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<SaveQualificationFundingOffersDetailsCommand>(), default))
                         .ReturnsAsync(commandResponse);

            // Act
            var result = await controller.SaveFundingOfferDetails(command, Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task CreateQualificationDiscussionHistoryNoteForFundingOffers_ReturnsOkResult()
        {
            // Arrange
            var commandResponse = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
            commandResponse.Success = true;
            var controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
            var command = _fixture.Create<CreateQualificationDiscussionHistoryNoteForFundingOffersCommand>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateQualificationDiscussionHistoryNoteForFundingOffersCommand>(), default))
                         .ReturnsAsync(commandResponse);

            // Act
            var result = await controller.CreateQualificationDiscussionHistoryNoteForFundingOffers(command, Guid.NewGuid());

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

    }
    public class DateOnlySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DateOnly))
            {
                return new DateOnly(2023, 1, 1); //a valid default date here
            }

            return new NoSpecimen();
        }
    }
}
