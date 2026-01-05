using System;
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
using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;
using SFA.DAS.EarlyConnect.Services.Interfaces;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.ManageStudentTriageData;

public class WhenManagingStudentTriageData
{
    [Test]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        var earlyConnectApiClientMock = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
        var sendStudentDataToLepsServiceMock = new Mock<ISendStudentDataToLepsService>();
        var featureConfig = new Mock<IFeature>();
        var handler = new ManageStudentTriageDataCommandHandler(earlyConnectApiClientMock.Object, sendStudentDataToLepsServiceMock.Object, featureConfig.Object);

        var command = new ManageStudentTriageDataCommand
        {
            StudentTriageData = new StudentTriageData()
        };

        command.StudentTriageData.StudentSurvey = new StudentSurveyDto();

        command.StudentTriageData.StudentSurvey.DateCompleted = null;

        var expectedResponse = new Mock<ManageStudentTriageDataResponse>();
        expectedResponse.Object.Message =String.Empty;
        var cancellationToken = new CancellationToken();

        var response = new ApiResponse<ManageStudentTriageDataResponse>(expectedResponse.Object, HttpStatusCode.OK, string.Empty);

        earlyConnectApiClientMock.Setup(c => c.PostWithResponseCode<ManageStudentTriageDataResponse>(It.IsAny<ManageStudentTriageDataRequest>(), true)).ReturnsAsync(response);
        var result = await handler.Handle(command, cancellationToken);

        earlyConnectApiClientMock.Verify(x => x.PostWithResponseCode<ManageStudentTriageDataResponse>(It.IsAny<ManageStudentTriageDataRequest>(), It.IsAny<bool>()), Times.Once);
        Assert.That(result.Message, Is.EqualTo(expectedResponse.Object.Message));
    }
}