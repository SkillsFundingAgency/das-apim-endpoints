﻿using System;
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
        public async Task SendStudentDataToLeps_WhenNorthEastRegionAndLepDateSentIsNull_CallsNeApiClient(GetStudentTriageDataResponse apiResponse)
        {
            var surveyGuid = Guid.NewGuid();
            var mockApiClient = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
            var mockNeApiClient = new Mock<ILepsNeApiClient<LepsNeApiConfiguration>>();
            var mockLaApiClient = new Mock<ILepsLaApiClient<LepsLaApiConfiguration>>();
            var mockLoApiClient = new Mock<ILepsLoApiClient<LepsLoApiConfiguration>>();
            var service = new SendStudentDataToLepsService(mockApiClient.Object, mockNeApiClient.Object, mockLaApiClient.Object, mockLoApiClient.Object);

            apiResponse.LepCode = LepsRegion.NorthEast;
            apiResponse.LepDateSent = null;

            foreach (var surveyQuestion in apiResponse.SurveyQuestions)
            {
                surveyQuestion.QuestionTypeId = 1;
            }

            var SendStudentDataToNeLepsexpectedResponse = new Mock<SendStudentDataToNeLepsResponse>();

            var SendStudentDataToNeLepsResponse = new ApiResponse<SendStudentDataToNeLepsResponse>(SendStudentDataToNeLepsexpectedResponse.Object, HttpStatusCode.OK, string.Empty);

            mockApiClient.Setup(x => x.GetWithResponseCode<GetStudentTriageDataResponse>(It.IsAny<GetStudentTriageDataBySurveyIdRequest>())).ReturnsAsync(new ApiResponse<GetStudentTriageDataResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var sendStudentDataResponse = new SendStudentDataToNeLepsResponse();
            mockNeApiClient.Setup(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false))
                .ReturnsAsync(SendStudentDataToNeLepsResponse);

            var result = await service.SendStudentDataToNe(surveyGuid);

            mockNeApiClient.Verify(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false), Times.Once);
            Assert.That(result, Is.Not.Null);
        }
        [Test, MoqAutoData]
        public async Task SendStudentDataToLeps_WhenLancashireRegionAndLepDateSentIsNull_CallsLaApiClient(GetStudentTriageDataResponse apiResponse)
        {
            var surveyGuid = Guid.NewGuid();
            var mockApiClient = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
            var mockNeApiClient = new Mock<ILepsNeApiClient<LepsNeApiConfiguration>>();
            var mockLaApiClient = new Mock<ILepsLaApiClient<LepsLaApiConfiguration>>();
            var mockLoApiClient = new Mock<ILepsLoApiClient<LepsLoApiConfiguration>>();
            var service = new SendStudentDataToLepsService(mockApiClient.Object, mockNeApiClient.Object, mockLaApiClient.Object, mockLoApiClient.Object);

            apiResponse.LepCode = LepsRegion.Lancashire;
            apiResponse.LepDateSent = null;

            foreach (var surveyQuestion in apiResponse.SurveyQuestions)
            {
                surveyQuestion.QuestionTypeId = 1;
            }

            var SendStudentDataToNeLepsexpectedResponse = new Mock<SendStudentDataToNeLepsResponse>();

            var SendStudentDataToNeLepsResponse = new ApiResponse<SendStudentDataToNeLepsResponse>(SendStudentDataToNeLepsexpectedResponse.Object, HttpStatusCode.OK, string.Empty);

            mockApiClient.Setup(x => x.GetWithResponseCode<GetStudentTriageDataResponse>(It.IsAny<GetStudentTriageDataBySurveyIdRequest>())).ReturnsAsync(new ApiResponse<GetStudentTriageDataResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var sendStudentDataResponse = new SendStudentDataToNeLepsResponse();
            mockNeApiClient.Setup(x => x.PostWithResponseCode<SendStudentDataToNeLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false))
                .ReturnsAsync(SendStudentDataToNeLepsResponse);

            var result = await service.SendStudentDataToLa(surveyGuid);

            mockLaApiClient.Verify(x => x.PostWithResponseCode<SendStudentDataToLaLepsResponse>(It.IsAny<SendStudentDataToLaLepsRequest>(), false), Times.Once);
            Assert.That(result, Is.Not.Null);
        }
        public async Task SendStudentDataToLeps_WhenLondonRegionAndLepDateSentIsNull_CallsLaApiClient(GetStudentTriageDataResponse apiResponse)
        {
            var surveyGuid = Guid.NewGuid();
            var mockApiClient = new Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>>();
            var mockNeApiClient = new Mock<ILepsNeApiClient<LepsNeApiConfiguration>>();
            var mockLaApiClient = new Mock<ILepsLaApiClient<LepsLaApiConfiguration>>();
            var mockLoApiClient = new Mock<ILepsLoApiClient<LepsLoApiConfiguration>>();
            var service = new SendStudentDataToLepsService(mockApiClient.Object, mockNeApiClient.Object, mockLaApiClient.Object, mockLoApiClient.Object);

            apiResponse.LepCode = LepsRegion.London;
            apiResponse.LepDateSent = null;

            foreach (var surveyQuestion in apiResponse.SurveyQuestions)
            {
                surveyQuestion.QuestionTypeId = 1;
            }

            var SendStudentDataToLoLepsexpectedResponse = new Mock<SendStudentDataToLoLepsResponse>();

            var SendStudentDataToLoLepsResponse = new ApiResponse<SendStudentDataToLoLepsResponse>(SendStudentDataToLoLepsexpectedResponse.Object, HttpStatusCode.OK, string.Empty);

            mockApiClient.Setup(x => x.GetWithResponseCode<GetStudentTriageDataResponse>(It.IsAny<GetStudentTriageDataBySurveyIdRequest>())).ReturnsAsync(new ApiResponse<GetStudentTriageDataResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var sendStudentDataResponse = new SendStudentDataToNeLepsResponse();
            mockNeApiClient.Setup(x => x.PostWithResponseCode<SendStudentDataToLoLepsResponse>(It.IsAny<SendStudentDataToNeLepsRequest>(), false))
                .ReturnsAsync(SendStudentDataToLoLepsResponse);

            var result = await service.SendStudentDataToLo(surveyGuid);

            mockLoApiClient.Verify(x => x.PostWithResponseCode<SendStudentDataToLoLepsResponse>(It.IsAny<SendStudentDataToLoLepsRequest>(), false), Times.Once);
            Assert.That(result, Is.Not.Null);
        }
    }
}
