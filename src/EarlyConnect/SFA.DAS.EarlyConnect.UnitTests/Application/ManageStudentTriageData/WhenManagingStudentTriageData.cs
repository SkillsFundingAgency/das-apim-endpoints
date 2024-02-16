using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.ManageStudentTriageData;

public class WhenManagingStudentTriageData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new ManageStudentTriageDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new ManageStudentTriageDataCommand
        {
            StudentTriageData = new StudentTriageData()
        };

        var expectedResponse = new Mock<ManageStudentTriageDataResponse>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<ManageStudentTriageDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<ManageStudentTriageDataResponse>(It.IsAny<ManageStudentTriageDataRequest>(), true)).ReturnsAsync(response);
        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<ManageStudentTriageDataResponse>(It.IsAny<ManageStudentTriageDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(result.Message, Is.EqualTo(expectedResponse.Object.Message));
    }
}