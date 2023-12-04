using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.CreateLog;

public class WhenCreatingLogData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateLogDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateLogDataCommand
        {
            Log = new LogCreate()
        };

        var expectedResponse = new Mock<CreateLogDataResponse>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<CreateLogDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<CreateLogDataResponse>(It.IsAny<CreateLogDataRequest>(), true)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<CreateLogDataResponse>(It.IsAny<CreateLogDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(expectedResponse.Object.LogId, result.LogId);
    }
}