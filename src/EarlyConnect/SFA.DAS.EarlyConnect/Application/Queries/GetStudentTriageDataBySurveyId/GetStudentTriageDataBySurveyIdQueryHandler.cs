using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQueryHandler : IRequestHandler<GetStudentTriageDataBySurveyIdQuery, StudentTriageDataShared>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataBySurveyIdQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<StudentTriageDataShared> Handle(GetStudentTriageDataBySurveyIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<StudentTriageDataShared>(new GetStudentTriageDataBySurveyIdRequest(request.SurveyGuid));

            result.EnsureSuccessStatusCode();

            var test = new StudentSurveyDtoShared
            {
                Id = result.Body.StudentSurvey.Id,
                StudentId = result.Body.StudentSurvey.StudentId,
                SurveyId = result.Body.StudentSurvey.SurveyId,
                LastUpdated = result.Body.StudentSurvey.LastUpdated,
                DateCompleted = result.Body.StudentSurvey.DateCompleted,
                DateEmailSent = result.Body.StudentSurvey.DateEmailSent,
                DateAdded = result.Body.StudentSurvey.DateAdded,
                ResponseAnswers = result.Body.StudentSurvey.ResponseAnswers,
            };

            return new StudentTriageDataShared
            {
                Id = result.Body.Id,
                LepDateSent = result.Body.LepDateSent,
                LepsId = result.Body.LepsId,
                LepCode = result.Body.LepCode,
                LogId = result.Body.LogId,
                FirstName = result.Body.FirstName,
                LastName = result.Body.LastName,
                DateOfBirth = result.Body.DateOfBirth,
                SchoolName = result.Body.SchoolName,
                URN=result.Body.URN,
                Email = result.Body.Email,
                Telephone = result.Body.Telephone,
                Postcode = result.Body.Postcode,
                DataSource = result.Body.DataSource,
                Industry = result.Body.Industry,
                DateInterest = result.Body.DateInterest,
                StudentSurvey = test,
                SurveyQuestions = (ICollection<SurveyQuestionsDtoShared>)result.Body.SurveyQuestions
            };
        }
    }
}