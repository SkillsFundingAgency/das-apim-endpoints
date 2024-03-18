using System.Net;
using SFA.DAS.EarlyConnect.Configuration;
using SFA.DAS.EarlyConnect.ExternalApi.Requests;
using SFA.DAS.EarlyConnect.ExternalApi.Responses;
using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
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
        private readonly ILepsLaApiClient<LepsLaApiConfiguration> _apiLepsLaClient;
        private SendStudentDataToLepsServiceResponse _sendStudentDataToLepsServiceResponse;

        public SendStudentDataToLepsService(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient,
            ILepsNeApiClient<LepsNeApiConfiguration> apiLepsNeClient,
            ILepsLaApiClient<LepsLaApiConfiguration> apiLepsLaClient)
        {
            _apiClient = apiClient;
            _apiLepsNeClient = apiLepsNeClient;
            _apiLepsLaClient = apiLepsLaClient;
        }
        public async Task<SendStudentDataToLepsServiceResponse> SendStudentDataToLeps(Guid SurveyGuid, LepsRegion.Region region)
        {
            var getStudentTriageResult = await _apiClient.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(SurveyGuid));
            getStudentTriageResult.EnsureSuccessStatusCode();

            _sendStudentDataToLepsServiceResponse = new SendStudentDataToLepsServiceResponse();

            if (getStudentTriageResult.Body.LepsId == (int)LepsRegion.Region.NorthEast &&
                getStudentTriageResult.Body.LepDateSent == null)
            {
                return await SendToNorthEast(getStudentTriageResult.Body, SurveyGuid);

            }

            if (getStudentTriageResult.Body.LepsId == (int)LepsRegion.Region.Lancashire &&
                getStudentTriageResult.Body.LepDateSent == null)
            {
                return await SendToLancashire(getStudentTriageResult.Body, SurveyGuid);
            }

            return CreateSendStudentDataToLepsServiceResponse("Leps not enabled");
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

            return _sendStudentDataToLepsServiceResponse;
        }

        private async Task<SendStudentDataToLepsServiceResponse> SendToLancashire(GetStudentTriageDataBySurveyIdResponse data, Guid surveyGuid)
        {
            var studentTriageData = MapResponseToStudentData(data);

            var sendStudentDataresult =
                await _apiLepsLaClient.PostWithResponseCode<SendStudentDataToLaLepsResponse>(
                    new SendStudentDataToLaLepsRequest(studentTriageData, surveyGuid), false);

            if (sendStudentDataresult == null || sendStudentDataresult.StatusCode != HttpStatusCode.Created)
            {
                return CreateSendStudentDataToLepsServiceResponse($"{sendStudentDataresult?.Body?.Message}");
            }

            await PerformDeliveryUpdate(data.Id);

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
