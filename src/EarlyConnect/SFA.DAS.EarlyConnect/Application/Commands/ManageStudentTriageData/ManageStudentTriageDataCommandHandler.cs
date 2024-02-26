using System.Net;
using MediatR;
using SFA.DAS.EarlyConnect.Configuration;
using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;
using SFA.DAS.EarlyConnect.ExternalApi.Requests;
using SFA.DAS.EarlyConnect.ExternalApi.Responses;
using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.EarlyConnect.Web.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData
{
    public class ManageStudentTriageDataCommandHandler : IRequestHandler<ManageStudentTriageDataCommand, ManageStudentTriageDataCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;
        private readonly ILepsNeApiClient<LepsNeApiConfiguration> _apiLepsClient;
        private readonly IFeature _feature;

        public ManageStudentTriageDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient, ILepsNeApiClient<LepsNeApiConfiguration> apiLepsClient, IFeature feature)
        {
            _apiClient = apiClient;
            _apiLepsClient = apiLepsClient;
            _feature = feature;
        }

        public async Task<ManageStudentTriageDataCommandResult> Handle(ManageStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var manageStudentResponse = await _apiClient.PostWithResponseCode<ManageStudentTriageDataResponse>(new ManageStudentTriageDataRequest(request.StudentTriageData, request.SurveyGuid), true);

            manageStudentResponse.EnsureSuccessStatusCode();

            if (request.StudentTriageData.StudentSurvey.DateCompleted == null)
            {
                return new ManageStudentTriageDataCommandResult
                {
                    Message = $"{manageStudentResponse?.Body?.Message}"
                };
            }

            if (_feature.IsFeatureEnabled(FeatureNames.NorthEastDataSharing))
            {
                var getStudentTriageResult = await _apiClient.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(request.SurveyGuid));

                getStudentTriageResult.EnsureSuccessStatusCode();

                var isSurveyCompleted = (getStudentTriageResult.Body.StudentSurvey.DateCompleted != null);

                if (isSurveyCompleted)
                {
                    return new ManageStudentTriageDataCommandResult
                    {
                        Message = $"{manageStudentResponse?.Body?.Message}"
                    };
                }

                var studentTriageData = MapResponseToStudentData(getStudentTriageResult.Body);

                var sendStudentDataresult = await _apiLepsClient.PostWithResponseCode<SendStudentDataToNeLepsResponse>(new SendStudentDataToNeLepsRequest(studentTriageData, request.SurveyGuid), false);

                if (sendStudentDataresult == null || sendStudentDataresult.StatusCode != HttpStatusCode.Created)
                {
                    return new ManageStudentTriageDataCommandResult
                    {
                        Message = $"{manageStudentResponse?.Body?.Message} - {sendStudentDataresult?.Body?.Message}"
                    };
                }

                var deliveryUpdate = new DeliveryUpdate { Source = DataSource.StudentData, Ids = new List<int> { getStudentTriageResult.Body.Id } };

                var deliveryUpdateresponse = await _apiClient.PostWithResponseCode<DeliveryUpdateDataResponse>(new DeliveryUpdateRequest(deliveryUpdate));

                deliveryUpdateresponse.EnsureSuccessStatusCode();

                return new ManageStudentTriageDataCommandResult
                {
                    Message = $"{manageStudentResponse?.Body?.Message} - {sendStudentDataresult?.Body?.Message} - {deliveryUpdateresponse?.Body?.Message}"
                };
            }

            return new ManageStudentTriageDataCommandResult
            {
                Message = $"{manageStudentResponse?.Body?.Message}"
            };
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
}


