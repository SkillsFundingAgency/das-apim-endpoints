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
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.OtherStudentTriageData;

public class WhenCreatingOtherStudentTriageData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateOtherStudentTriageDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateOtherStudentTriageDataCommand
        {
            StudentTriageData = new EarlyConnect.InnerApi.Requests.OtherStudentTriageData()
        };


        var expectedResponse = new Mock<CreateOtherStudentTriageDataResponse>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<CreateOtherStudentTriageDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<CreateOtherStudentTriageDataResponse>(It.IsAny<CreateOtherStudentTriageDataRequest>(), true)).ReturnsAsync(response);
        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<CreateOtherStudentTriageDataResponse>(It.IsAny<CreateOtherStudentTriageDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.AreEqual(expectedResponse.Object.StudentSurveyId, result.StudentSurveyId);
        Assert.AreEqual(expectedResponse.Object.AuthCode, result.AuthCode);
        Assert.AreEqual(expectedResponse.Object.ExpiryDate, result.ExpiryDate);
    }
}