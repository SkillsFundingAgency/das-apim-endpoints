using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.FormBuilder;

[TestFixture]
public class RoutesControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<RoutesController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private RoutesController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<RoutesController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new RoutesController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task GetAvailableSectionsAndPagesForRouting_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQuery>();
        var response = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQueryResponse>();
        BaseMediatrResponse<GetAvailableSectionsAndPagesForRoutingQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAvailableSectionsAndPagesForRoutingQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAvailableSectionsAndPagesForRouting(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetAvailableSectionsAndPagesForRoutingQueryResponse>());
        var model = (GetAvailableSectionsAndPagesForRoutingQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetRoutesByFormVersionId_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetRoutingInformationForFormQuery>();
        var response = new GetRoutingInformationForFormQueryResponse()
        {
            Sections = new(),
            Editable = true
        };
        BaseMediatrResponse<GetRoutingInformationForFormQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoutingInformationForFormQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetRoutesByFormVersionId(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetRoutingInformationForFormQueryResponse>());
        var model = (GetRoutingInformationForFormQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetAvailableQuestionsForRouting_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetAvailableQuestionsForRoutingQuery>();
        var response = _fixture.Create<GetAvailableQuestionsForRoutingQueryResponse>();
        BaseMediatrResponse<GetAvailableQuestionsForRoutingQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAvailableQuestionsForRoutingQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAvailableQuestionsForRouting(request.PageId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetAvailableQuestionsForRoutingQueryResponse>());
        var model = (GetAvailableQuestionsForRoutingQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetQuestionRoutingInformation_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetRoutingInformationForQuestionQuery>();
        var response = _fixture.Create<GetRoutingInformationForQuestionQueryResponse>();
        BaseMediatrResponse<GetRoutingInformationForQuestionQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoutingInformationForQuestionQuery>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.GetQuestionRoutingInformation(request.QuestionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetRoutingInformationForQuestionQueryResponse>());
        var model = (GetRoutingInformationForQuestionQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task ConfigureRoutingForQuestion_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<ConfigureRoutingForQuestionCommand>();
        var response = _fixture.Create<ConfigureRoutingForQuestionCommandResponse>();
        BaseMediatrResponse<ConfigureRoutingForQuestionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ConfigureRoutingForQuestionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.ConfigureRoutingForQuestion(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<ConfigureRoutingForQuestionCommandResponse>());
        var model = (ConfigureRoutingForQuestionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }
}