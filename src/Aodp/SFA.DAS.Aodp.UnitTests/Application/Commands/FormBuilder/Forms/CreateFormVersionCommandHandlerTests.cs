using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Forms;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.FormBuilder.Forms;

[TestFixture]
public class CreateFormVersionCommandHandlerTests
{
    private IFixture _fixture;
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
    private CreateFormVersionCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        _handler = _fixture.Create<CreateFormVersionCommandHandler>();
    }

    [Test]
    public async Task Then_Success_Is_Returned_With_Id()
    {
        var body = _fixture.Create<CreateFormVersionCommandResponse>();
        _apiClientMock
            .Setup(a => a.PostWithResponseCode<CreateFormVersionCommandResponse>(It.IsAny<CreateFormVersionApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<CreateFormVersionCommandResponse>(body, HttpStatusCode.OK, ""));

        var result = await _handler.Handle(_fixture.Create<CreateFormVersionCommand>(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Value.Id, Is.EqualTo(body.Id));
    }

    [Test]
    public async Task Then_BadRequest_From_Inner_Api_Is_Returned_As_BadRequest()
    {
        const string errorContent = "{\"errors\":{\"Title\":[\"Title contains invalid characters\"]}}";
        _apiClientMock
            .Setup(a => a.PostWithResponseCode<CreateFormVersionCommandResponse>(It.IsAny<CreateFormVersionApiRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<CreateFormVersionCommandResponse>(null, HttpStatusCode.BadRequest, errorContent));

        var result = await _handler.Handle(_fixture.Create<CreateFormVersionCommand>(), CancellationToken.None);

        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorCode, Is.EqualTo(ErrorCodes.BadRequest));
        Assert.That(result.ErrorMessage, Is.EqualTo(errorContent));
    }
}

