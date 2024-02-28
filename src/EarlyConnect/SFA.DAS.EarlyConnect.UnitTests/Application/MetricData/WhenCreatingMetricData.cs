using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateMetricData;
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
            metricsData = new MetricDataList()
        };


        var cancellationToken = new CancellationToken();


        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<object>(It.IsAny<CreateMetricDataRequest>(), false)).ReturnsAsync(new ApiResponse<object> (new object(),HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<object>(It.IsAny<CreateMetricDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(Unit.Value, Is.EqualTo(result));
    }
}