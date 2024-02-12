using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using AutoFixture;
using System.Collections.Generic;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentOnboardData;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.StudentData;

public class WhenCreatingStudentData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var fixture = new Fixture();
        var studentDataList = fixture.Create<List<EarlyConnect.InnerApi.Requests.StudentData>>();

        var mediatorMock = new Mock<IMediator>();
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var handler = new CreateStudentDataCommandHandler(earlyConnectApiClientMock.Object, mediatorMock.Object);

        var command = new CreateStudentDataCommand
        {
            StudentDataList = new StudentDataList()
        };

        command.StudentDataList.ListOfStudentData = studentDataList;

        var expectedResponse = fixture.Create<CreateStudentDataResponse>();
        var cancellationToken = CancellationToken.None;

        var createStudentOnboardDataCommandResult = new CreateStudentOnboardDataCommandResult { Message = "Test" };

        var response = new ApiResponse<CreateStudentDataResponse>(expectedResponse, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<CreateStudentDataResponse>(
            It.IsAny<CreateStudentDataRequest>(), true)).ReturnsAsync(response);

        mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentOnboardDataCommand>(), cancellationToken))
            .ReturnsAsync(createStudentOnboardDataCommandResult);

        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<CreateStudentDataResponse>(
            It.IsAny<CreateStudentDataRequest>(), true), Times.Once);

        Assert.AreEqual($"{expectedResponse.Message} - {createStudentOnboardDataCommandResult.Message}", result.Message);
    }


}