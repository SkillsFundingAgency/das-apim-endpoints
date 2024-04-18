using System.Net;
using SFA.DAS.EarlyConnect.Configuration;
using SFA.DAS.EarlyConnect.ExternalApi.Requests;
using SFA.DAS.EarlyConnect.ExternalApi.Responses;
using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.EarlyConnect.Services.Configuration;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Services
{
    public class SendStudentDataToLepsService : ISendStudentDataToLepsService
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;
        private readonly ILepsNeApiClient<LepsNeApiConfiguration> _apiLepsNeClient;
        private readonly ILepsLoApiClient<LepsLoApiConfiguration> _apiLepsLoClient;
        private SendStudentDataToLepsServiceResponse _sendStudentDataToLepsServiceResponse;

        public SendStudentDataToLepsService(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient,
            ILepsNeApiClient<LepsNeApiConfiguration> apiLepsNeClient,
            ILepsLoApiClient<LepsLoApiConfiguration> apiLepsLoClient)
        {
            _apiClient = apiClient;
            _apiLepsNeClient = apiLepsNeClient;
            _apiLepsLoClient = apiLepsLoClient;
        }
        public async Task<SendStudentDataToLepsServiceResponse> SendStudentDataToNe(Guid SurveyGuid)
        {
            var getStudentTriageResult = await _apiClient.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(SurveyGuid));
            getStudentTriageResult.EnsureSuccessStatusCode();

            _sendStudentDataToLepsServiceResponse = new SendStudentDataToLepsServiceResponse();

            if (getStudentTriageResult.Body.LepCode.ToUpper() == LepsRegion.NorthEast && getStudentTriageResult.Body.LepDateSent == null)
            {
                return await SendToNorthEast(getStudentTriageResult.Body, SurveyGuid);

            }

            return CreateSendStudentDataToLepsServiceResponse("LepCode not matching with Ne");
        }

        public async Task<SendStudentDataToLepsServiceResponse> SendStudentDataToLo(Guid SurveyGuid)
        {
            var getStudentTriageResult = await _apiClient.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(SurveyGuid));
            getStudentTriageResult.EnsureSuccessStatusCode();

            _sendStudentDataToLepsServiceResponse = new SendStudentDataToLepsServiceResponse();

            if (getStudentTriageResult.Body.LepCode.ToUpper() == LepsRegion.London && getStudentTriageResult.Body.LepDateSent == null)
            {
                return await SendToLondon(getStudentTriageResult.Body, SurveyGuid);
            }

            return CreateSendStudentDataToLepsServiceResponse("LepCode not matching with Lo");
        }

        private async Task<SendStudentDataToLepsServiceResponse> SendToNorthEast(GetStudentTriageDataBySurveyIdResponse data, Guid surveyGuid)
        {
            var studentTriageData = MapResponseToStudentData(data);

            var sendStudentDataresult =
                await _apiLepsNeClient.PostWithResponseCode<SendStudentDataToNeLepsResponse>(
                    new SendStudentDataToNeLepsRequest(studentTriageData, surveyGuid), false);

            if (sendStudentDataresult == null || sendStudentDataresult.StatusCode != HttpStatusCode.Created)
            {
                return CreateSendStudentDataToLepsServiceResponse($"{sendStudentDataresult?.Body?.Message}");
            }

            await PerformDeliveryUpdate(data.Id);

            CreateSendStudentDataToLepsServiceResponse("Ne process completed");

            return _sendStudentDataToLepsServiceResponse;
        }

        private async Task<SendStudentDataToLepsServiceResponse> SendToLondon(GetStudentTriageDataBySurveyIdResponse data, Guid surveyGuid)
        {
            var studentTriageData = MapResponseToStudentData(data);

            var sendStudentDataresult =
                await _apiLepsLoClient.PostWithResponseCode<SendStudentDataToLoLepsResponse>(
                    new SendStudentDataToLoLepsRequest(studentTriageData, surveyGuid), false);

            if (sendStudentDataresult == null || (sendStudentDataresult.StatusCode != HttpStatusCode.Created && sendStudentDataresult.StatusCode != HttpStatusCode.OK))
            {
                return CreateSendStudentDataToLepsServiceResponse($"{sendStudentDataresult?.Body?.Message}");
            }

            await PerformDeliveryUpdate(data.Id);

            CreateSendStudentDataToLepsServiceResponse("Lo process completed");

            return _sendStudentDataToLepsServiceResponse;
        }

        private async Task PerformDeliveryUpdate(int studentDataId)
        {
            var deliveryUpdate = new DeliveryUpdate { Source = DataSource.StudentData, Ids = new List<int> { studentDataId } };
            var deliveryUpdateResponse = await _apiClient.PostWithResponseCode<DeliveryUpdateDataResponse>(new DeliveryUpdateRequest(deliveryUpdate));
            deliveryUpdateResponse.EnsureSuccessStatusCode();
            CreateSendStudentDataToLepsServiceResponse($"{deliveryUpdateResponse?.Body?.Message}");
        }

        public StudentTriageData MapResponseToStudentData(GetStudentTriageDataBySurveyIdResponse responseData)
        {
            var studentData = new StudentTriageData
            {
                Id = responseData.Id,
                LepsId = responseData.LepsId,
                LogId = responseData.LogId,
                FirstName = responseData.FirstName,
                LastName = responseData.LastName,
                DateOfBirth = responseData.DateOfBirth?.Date,
                Email = responseData.Email,
                Telephone = responseData.Telephone,
                Postcode = responseData.Postcode,
                DataSource = responseData.DataSource,
                SchoolName = responseData.SchoolName,
                Industry = responseData.Industry,
                DateInterest = responseData.DateInterest,
                SurveyResponses = responseData.SurveyQuestions.Select(question => new SurveyQuestionsDto
                {
                    QuestionId = question.Id,
                    QuestionType = GetQuestionType(question.QuestionTypeId),
                    QuestionText = question.QuestionText,
                    ShortDescription = question.ShortDescription,
                    SummaryLabel = question.SummaryLabel,
                    SortOrder = question.SortOrder,
                    Answers = MapAnswers(responseData, question.Id).ToList(),
                    StudentResponse = MapStudentResponses(responseData.StudentSurvey.ResponseAnswers, responseData, question.Id).ToList()
                }).ToList(),
                StudentSurvey = new StudentSurveyDto
                {
                    Id = responseData.StudentSurvey.Id,
                    StudentId = responseData.StudentSurvey.StudentId,
                    SurveyId = responseData.StudentSurvey.SurveyId,
                    LastUpdated = responseData.StudentSurvey.LastUpdated,
                    DateCompleted = responseData.StudentSurvey.DateCompleted,
                    DateEmailSent = responseData.StudentSurvey.DateEmailSent,
                    DateAdded = responseData.StudentSurvey.DateAdded,
                }
            };

            return studentData;
        }

        private IEnumerable<AnswersDto> MapAnswers(GetStudentTriageDataBySurveyIdResponse responseData, int questionId)
        {
            return responseData.SurveyQuestions
                .Where(q => q.Id == questionId)
                .SelectMany(q => q.Answers.Select(answer => new AnswersDto
                {
                    Id = answer.Id,
                    QuestionId = questionId,
                    AnswerText = answer.AnswerText,
                    ShortDescription = answer.ShortDescription,
                }));
        }

        private IEnumerable<ResponseAnswersDto> MapStudentResponses(ICollection<SFA.DAS.EarlyConnect.Models.ResponseAnswersDto> responseAnswers, GetStudentTriageDataBySurveyIdResponse responseData, int questionId)
        {
            return responseAnswers
                .Where(responseAnswer => responseAnswer.QuestionId == questionId)
                .Select(responseAnswer => new ResponseAnswersDto
                {
                    Id = responseAnswer.Id,
                    QuestionId = questionId,
                    AnswerId = responseAnswer.AnswerId,
                    AnswerText = GetAnswerText(responseAnswer.AnswerId.Value, questionId, responseData),
                    Response = responseAnswer.Response,
                    DateAdded = responseAnswer.DateAdded
                });
        }

        private string GetAnswerText(int answerId, int questionId, GetStudentTriageDataBySurveyIdResponse responseData)
        {
            var question = responseData.SurveyQuestions.FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);
                if (answer != null)
                {
                    return answer.AnswerText;
                }
            }

            return "Answer Not Found";
        }

        private SendStudentDataToLepsServiceResponse CreateSendStudentDataToLepsServiceResponse(string message)
        {
            _sendStudentDataToLepsServiceResponse.Message = string.IsNullOrEmpty(_sendStudentDataToLepsServiceResponse.Message) ? message : $"{_sendStudentDataToLepsServiceResponse.Message}- {message}";

            return _sendStudentDataToLepsServiceResponse;
        }

        private string GetQuestionType(int questionTypeId)
        {
            switch (questionTypeId)
            {
                case 1:
                    return SurveyQuestionType.Type.MultipleChoice.ToString();
                case 2:
                    return SurveyQuestionType.Type.Radio.ToString();
                case 3:
                    return SurveyQuestionType.Type.FreeText.ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(questionTypeId), $"Unknown question type ID: {questionTypeId}");
            }
        }
    }

    public class SendStudentDataToLepsServiceResponse
    {
        public string Message { get; set; }
    }
}
