using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLog;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.CreateLog;

public class WhenCreatingLog
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateLogCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateLogCommand
        {
            Log = new EarlyConnect.InnerApi.Requests.CreateLog()
        };

        var expectedResponse = new Mock<EarlyConnect.InnerApi.Requests.CreateLog>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<EarlyConnect.InnerApi.Requests.CreateLog>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<EarlyConnect.InnerApi.Requests.CreateLog>(It.IsAny<CreateLogRequest>(), false)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<EarlyConnect.InnerApi.Requests.CreateLog>(It.IsAny<CreateLogRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(Unit.Value, result);
    }
}