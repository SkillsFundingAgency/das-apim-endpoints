using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.UpdateLog;

public class WhenUpdatingLogData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new UpdateLogDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new UpdateLogDataCommand
        {
            Log = new LogUpdate()
        };


        var cancellationToken = new CancellationToken();


        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<object>(It.IsAny<UpdateLogRequest>(), false)).ReturnsAsync(new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<object>(It.IsAny<UpdateLogRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(Unit.Value, result);
    }
}