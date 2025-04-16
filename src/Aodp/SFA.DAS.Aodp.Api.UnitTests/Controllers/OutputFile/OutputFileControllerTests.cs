using AutoFixture.AutoMoq;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Aodp.Api.Controllers.OutputFile;
using SFA.DAS.AODP.Application.Queries.OutputFile;
using Azure;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;
using Azure.Core;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Commands.OutputFile;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.OutputFile;

[TestFixture]
public class OutputFileControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<OutputFileController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private OutputFileController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<OutputFileController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new OutputFileController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task Get_OutputFileGenerationHistory_ReturnsOk_OnSucess()
    {
        var expectedResult = _fixture.Create<BaseMediatrResponse<GetPreviousOutputFilesQueryResponse>>();
        _mediatorMock.Setup(v => v.Send(It.IsAny<GetPreviousOutputFilesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expectedResult));

        var result = await _controller.GetAllAsync();

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetPreviousOutputFilesQuery>(), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetPreviousOutputFilesQueryResponse>());
        var model = (GetPreviousOutputFilesQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(expectedResult.Value));
    }

    [Test]
    public async Task Post_GenerateOutputFile_ReturnsOk_OnSucess()
    {
        var expectedResult = _fixture.Create<BaseMediatrResponse<EmptyResponse>>();
        _mediatorMock.Setup(v => v.Send(It.IsAny<GenerateNewOutputFileCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expectedResult));

        var result = await _controller.CreateAsync(new GenerateNewOutputFileCommand());

        _mediatorMock.Verify(m => m.Send(It.IsAny<GenerateNewOutputFileCommand>(), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
    }
}
