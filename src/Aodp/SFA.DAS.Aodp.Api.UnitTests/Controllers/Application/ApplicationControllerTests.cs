using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Application;
[TestFixture]
public class ApplicationControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<Api.Controllers.Application.ApplicationsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private Api.Controllers.Application.ApplicationsController _controller;


    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<Api.Controllers.Application.ApplicationsController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new Api.Controllers.Application.ApplicationsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        // Arrange
        var response = _fixture.Create<GetApplicationFormsQueryResponse>();
        BaseMediatrResponse<GetApplicationFormsQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationFormsQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationFormsQuery>(), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationFormsQueryResponse>());
        var model = (GetApplicationFormsQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationMetadataByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationMetadataByIdQuery>();
        var response = _fixture.Create<GetApplicationMetadataByIdQueryResponse>();
        BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationMetadataByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationMetadataByIdAsync(request.ApplicationId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationMetadataByIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationMetadataByIdQuery>(q =>
                    q.ApplicationId == request.ApplicationId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationMetadataByIdQueryResponse>());
        var model = (GetApplicationMetadataByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationPageByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationPageByIdQuery>();
        var response = new GetApplicationPageByIdQueryResponse
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "",
            Order = 0,
            TotalSectionPages = 0,
            Questions = new(),
        };
        BaseMediatrResponse<GetApplicationPageByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationPageByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetByIdAsync(request.PageOrder, request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationPageByIdQueryResponse>());
        var model = (GetApplicationPageByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationFormByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationFormByIdQuery>();
        var response = _fixture.Create<GetApplicationFormByIdQueryResponse>();
        BaseMediatrResponse<GetApplicationFormByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationFormByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetFormVersionByIdAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationFormByIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationFormByIdQuery>(q =>
                    q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationFormByIdQueryResponse>());
        var model = (GetApplicationFormByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationByOrganisationIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationsByOrganisationIdQuery>();
        var response = _fixture.Create<GetApplicationsByOrganisationIdQueryResponse>();
        BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationsByOrganisationIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationsByOrganisationId(request.OrganisationId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationsByOrganisationIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationsByOrganisationIdQuery>(q =>
                    q.OrganisationId == request.OrganisationId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationsByOrganisationIdQueryResponse>());
        var model = (GetApplicationsByOrganisationIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationSectionByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationFormStatusByApplicationIdQuery>();
        var response = _fixture.Create<GetApplicationFormStatusByApplicationIdQueryResponse>();
        BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationFormStatusByApplicationIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationSectionsForFormByIdAsync(request.ApplicationId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationFormStatusByApplicationIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationFormStatusByApplicationIdQuery>(q =>
                    q.ApplicationId == request.ApplicationId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationFormStatusByApplicationIdQueryResponse>());
        var model = (GetApplicationFormStatusByApplicationIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationSectionStatusByApplicationIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationSectionStatusByApplicationIdQuery>();
        var response = _fixture.Create<GetApplicationSectionStatusByApplicationIdQueryResponse>();
        BaseMediatrResponse<GetApplicationSectionStatusByApplicationIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationSectionStatusByApplicationIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationPagesForSectionByIdAsync(request.ApplicationId, request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationSectionStatusByApplicationIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationSectionStatusByApplicationIdQuery>(q =>
                    q.ApplicationId == request.ApplicationId
                    && q.SectionId == request.SectionId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once()); 
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationSectionStatusByApplicationIdQueryResponse>());
        var model = (GetApplicationSectionStatusByApplicationIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationFormStatusByApplicationIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationFormStatusByApplicationIdQuery>();
        var response = _fixture.Create<GetApplicationFormStatusByApplicationIdQueryResponse>();
        BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationFormStatusByApplicationIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationSectionsForFormByIdAsync(request.ApplicationId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationFormStatusByApplicationIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetApplicationFormStatusByApplicationIdQuery>(q =>
                    q.ApplicationId == request.ApplicationId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationFormStatusByApplicationIdQueryResponse>());
        var model = (GetApplicationFormStatusByApplicationIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetApplicationPageAnswersByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetApplicationPageAnswersByPageIdQuery>();
        var response = new GetApplicationPageAnswersByPageIdQueryResponse
        {
            Questions = new()
        };

        BaseMediatrResponse<GetApplicationPageAnswersByPageIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationPageAnswersByPageIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetApplicationPageAnswersByIdAsync(request.ApplicationId, request.PageId, request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationPageAnswersByPageIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m => 
            m.Send(
                It.Is<GetApplicationPageAnswersByPageIdQuery>(q => 
                    q.ApplicationId == request.ApplicationId 
                    && q.PageId == request.PageId 
                    && q.SectionId == request.SectionId 
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationPageAnswersByPageIdQueryResponse>());
        var model = (GetApplicationPageAnswersByPageIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreateApplicationCommand>();
        var response = _fixture.Create<CreateApplicationCommandResponse>();
        BaseMediatrResponse<CreateApplicationCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreateApplicationCommandResponse>());
        var model = (CreateApplicationCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new UpdatePageAnswersCommand()
        {
            ApplicationId = Guid.NewGuid(),
            PageId = Guid.NewGuid(),
            FormVersionId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            Questions = new(),
            Routing = new UpdatePageAnswersCommand.Route()
        };

        var response = _fixture.Create<UpdatePageAnswersCommandResponse>();
        BaseMediatrResponse<UpdatePageAnswersCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdatePageAnswersCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.UpdateAnswersAsync(request.FormVersionId, request.SectionId, request.PageId, request.ApplicationId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<UpdatePageAnswersCommandResponse>());
        var model = (UpdatePageAnswersCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task RemoveAsync_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = _fixture.Create<EmptyResponse>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.DeleteApplicationByIdAsync(id);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteApplicationCommand>(), default), Times.Once());
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteApplicationCommand>(c => c.ApplicationId == id), default), Times.Once());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
        var model = (EmptyResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task EditAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<EditApplicationCommand>();
        var response = _fixture.Create<EditApplicationCommandResponse>();
        BaseMediatrResponse<EditApplicationCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<EditApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.EditAsync(request, request.ApplicationId);

        // Assert
        Assert.Multiple(() =>
        {
            _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<EditApplicationCommandResponse>());
            var model = (EditApplicationCommandResponse)okResult.Value;
            Assert.That(model, Is.EqualTo(response));
        });
    }

    [Test]
    public async Task WithdrawApplicationByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<WithdrawApplicationCommand>();
        var response = _fixture.Create<EmptyResponse>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<WithdrawApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.WithdrawApplicationByIdAsync(request.ApplicationId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<WithdrawApplicationCommand>(c =>
                c.ApplicationId == request.ApplicationId), default), Times.Once());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
            var model = (EmptyResponse)okResult.Value;
            Assert.That(model, Is.EqualTo(response));
        });
    }
    
    [Test]
    public async Task WithdrawApplicationByIdAsync_WhenMediatorFails_ReturnsServerError()
    {
        // Arrange
        var request = _fixture.Create<WithdrawApplicationCommand>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Success = false,
            ErrorMessage = "Some failure"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<WithdrawApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.WithdrawApplicationByIdAsync(request.ApplicationId, request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = (StatusCodeResult)result;
            Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        });
    }

    [Test]
    public async Task WithdrawApplicationByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<WithdrawApplicationCommand>();
        var response = _fixture.Create<EmptyResponse>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<WithdrawApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.WithdrawApplicationByIdAsync(request.ApplicationId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<WithdrawApplicationCommand>(c =>
                c.ApplicationId == request.ApplicationId), default), Times.Once());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
            var model = (EmptyResponse)okResult.Value;
            Assert.That(model, Is.EqualTo(response));
        });
    }
    
    [Test]
    public async Task WithdrawApplicationByIdAsync_WhenMediatorFails_ReturnsServerError()
    {
        // Arrange
        var request = _fixture.Create<WithdrawApplicationCommand>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Success = false,
            ErrorMessage = "Some failure"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<WithdrawApplicationCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.WithdrawApplicationByIdAsync(request.ApplicationId, request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = (StatusCodeResult)result;
            Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        });
    }
}