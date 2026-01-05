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
using AutoFixture;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentOnboardData;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.StudentOnboardData;

public class WhenCreatingStudentOnboardData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var fixture = new Fixture();

        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateStudentOnboardDataCommandHandler(earlyConnectApiClientMock.Object);

        var command = new CreateStudentOnboardDataCommand
        {
            Emails = new EmailData()
        };

        var expectedResponse = fixture.Create<CreateStudentOnboardDataResponse>();
        var cancellationToken = CancellationToken.None;

        var response = new ApiResponse<CreateStudentOnboardDataResponse>(expectedResponse, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<CreateStudentOnboardDataResponse>(
            It.IsAny<CreateStudentOnboardDataRequest>(), true)).ReturnsAsync(response);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<CreateStudentOnboardDataResponse>(
            It.IsAny<CreateStudentOnboardDataRequest>(), true), Times.Once);

        Assert.That(result.Message, Is.EqualTo($"{expectedResponse.Message}"));
    }


}