using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.MetricData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.MetricData;

public class WhenCreatingMetricData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateMetricDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateMetricDataCommand
        {
            MetricDataList = new MetricDataList()
        };

        var expectedResponse = new Mock<EarlyConnect.InnerApi.Requests.MetricData>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<EarlyConnect.InnerApi.Requests.MetricData>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<EarlyConnect.InnerApi.Requests.MetricData>(It.IsAny<CreateMetricDataRequest>(), false)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<EarlyConnect.InnerApi.Requests.MetricData>(It.IsAny<CreateMetricDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(Unit.Value, result);
    }
}