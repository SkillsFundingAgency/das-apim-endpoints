using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.StudentData;

public class WhenCreatingStudentData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateStudentDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateStudentDataCommand
        {
            StudentDataList = new StudentDataList()
        };


        var expectedResponse = new Mock<CreateStudentDataResponse>();

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<CreateStudentDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<CreateStudentDataResponse>(It.IsAny<CreateStudentDataRequest>(), true)).ReturnsAsync(response);
        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<CreateStudentDataResponse>(It.IsAny<CreateStudentDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(expectedResponse.Object.Message, Is.EqualTo(result.Message));
    }
}