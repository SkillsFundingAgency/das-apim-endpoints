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
using SFA.DAS.EarlyConnect.Application.Commands.SendReminderEmail;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.CreateLog;

public class WhenSendingReminderEmail
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsUnit()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new SendReminderEmailCommandHandler(earlyConnectApiClientMock.Object);

        var command = new SendReminderEmailCommand
        {
            EmailReminder = new ReminderEmail()
        };

        var expectedResponse = new Mock<SendReminderEmailResponse>();

        expectedResponse.Object.Message = "Success";

        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<SendReminderEmailResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<SendReminderEmailResponse>(It.IsAny<SendReminderEmailRequest>(), true)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<SendReminderEmailResponse>(It.IsAny<SendReminderEmailRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(expectedResponse.Object.Message, Is.EqualTo(result.Message));
    }
}