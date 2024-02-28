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
using SFA.DAS.EarlyConnect.Application.Commands.DeliveryUpdateData;
using System.Collections.Generic;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.DeliveryData;

public class WhenUpdatingDeliveryData
{
    [Test]
    public async Task Handle_ValidRequest_Returns()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new DeliveryUpdateCommandHandler(earlyConnectApiClientMock.Object);

        var command = new DeliveryUpdateCommand
        {
            DeliveryUpdate = new DeliveryUpdate { Source = "StudentData", Ids = new List<int> { 1, 2, 27 } }
        };

        var expectedResponse = new Mock<DeliveryUpdateDataResponse>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<DeliveryUpdateDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<DeliveryUpdateDataResponse>(It.IsAny<DeliveryUpdateRequest>(), true)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<DeliveryUpdateDataResponse>(It.IsAny<DeliveryUpdateRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(result.Message, Is.EqualTo(expectedResponse.Object.Message));
    }
}