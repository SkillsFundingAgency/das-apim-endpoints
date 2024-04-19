using System;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.ExternalApi.Requests;
using SFA.DAS.EarlyConnect.ExternalApi.Responses;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.EarlyConnect.Services;
using SFA.DAS.EarlyConnect.Services.Configuration;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.UnitTests.Services
{
    [TestFixture]
    public class SendStudentDataToLepsServiceTests
    {
        [Test, MoqAutoData]
        public async Task SendStudentDataToLeps_WhenNorthEastRegionAndLepDateSentIsNull_CallsNeApiClient(GetStudentTriageDataBySurveyIdResponse apiResponse)
        {
            var surveyGuid = Guid.NewGuid();
            var mockApiClient = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
            var mockNeApiClient = new Mock<ILepsNeApiClient<LepsNeApiConfiguration>>();
            var mockLaApiClient = new Mock<ILepsLaApiClient<LepsLaApiConfiguration>>();
            var service = new SendStudentDataToLepsService(mockApiClient.Object, mockNeApiClient.Object, mockLaApiClient.Object);

            apiResponse.LepCode = LepsRegion.NorthEast;
            apiResponse.LepDateSent = null;

            foreach (var surveyQuestion in apiResponse.SurveyQuestions)
            {
                surveyQuestion.QuestionTypeId = 1;
            }

            var SendStudentDataToNeLepsexpectedResponse = new Mock<SendStudentDataToNeLepsResponse>();

            var SendStudentDataToNeLepsResponse = new ApiResponse<SendStudentDataToNeLepsResponse>(SendStudentDataToNeLepsexpectedResponse.Object, HttpStatusCode.OK, string.Empty);

            mockApiClient.Setup(x => x.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(It.IsAny<GetStudentTriageDataBySurveyIdRequest>())).ReturnsAsync(new ApiResponse<GetStudentTriageDataBySurveyIdResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var sendStudentDataResponse = new SendStudentDataToNeLepsResponse();
            mockNeApiClient.Setup(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false))
                .ReturnsAsync(SendStudentDataToNeLepsResponse);

            var result = await service.SendStudentDataToNe(surveyGuid);

            mockNeApiClient.Verify(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false), Times.Once);
            Assert.That(result, Is.Not.Null);
        }
        [Test, MoqAutoData]
        public async Task SendStudentDataToLeps_WhenLancashireRegionAndLepDateSentIsNull_CallsLaApiClient(GetStudentTriageDataBySurveyIdResponse apiResponse)
        {
            var surveyGuid = Guid.NewGuid();
            var mockApiClient = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
            var mockNeApiClient = new Mock<ILepsNeApiClient<LepsNeApiConfiguration>>();
            var mockLaApiClient = new Mock<ILepsLaApiClient<LepsLaApiConfiguration>>();
            var service = new SendStudentDataToLepsService(mockApiClient.Object, mockNeApiClient.Object, mockLaApiClient.Object);

            apiResponse.LepCode = LepsRegion.Lancashire;
            apiResponse.LepDateSent = null;

            foreach (var surveyQuestion in apiResponse.SurveyQuestions)
            {
                surveyQuestion.QuestionTypeId = 1;
            }

            var SendStudentDataToNeLepsexpectedResponse = new Mock<SendStudentDataToNeLepsResponse>();

            var SendStudentDataToNeLepsResponse = new ApiResponse<SendStudentDataToNeLepsResponse>(SendStudentDataToNeLepsexpectedResponse.Object, HttpStatusCode.OK, string.Empty);

            mockApiClient.Setup(x => x.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(It.IsAny<GetStudentTriageDataBySurveyIdRequest>())).ReturnsAsync(new ApiResponse<GetStudentTriageDataBySurveyIdResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var sendStudentDataResponse = new SendStudentDataToNeLepsResponse();
            mockNeApiClient.Setup(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false))
                .ReturnsAsync(SendStudentDataToNeLepsResponse);

            var result = await service.SendStudentDataToLa(surveyGuid);

            mockLaApiClient.Verify(x => x.PostWithResponseCode<SendStudentDataToLaLepsResponse>(It.IsAny<SendStudentDataToLaLepsRequest>(), false), Times.Once);
            Assert.That(result, Is.Not.Null);
        }
    }
}
