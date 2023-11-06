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
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLog;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.UpdateLog;

public class WhenUpdatingLog
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new UpdateLogCommandHandler(earlyConnectApiClientMock.Object);

        var command = new UpdateLogCommand
        {
            Log = new EarlyConnect.InnerApi.Requests.UpdateLog()
        };

        var expectedResponse = new Mock<EarlyConnect.InnerApi.Requests.UpdateLog>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<EarlyConnect.InnerApi.Requests.UpdateLog>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<EarlyConnect.InnerApi.Requests.UpdateLog>(It.IsAny<UpdateLogRequest>(), false)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<EarlyConnect.InnerApi.Requests.UpdateLog>(It.IsAny<UpdateLogRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(Unit.Value, result);
    }
}