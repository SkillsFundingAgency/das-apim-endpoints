using MediatR;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateQueryHandler : IRequestHandler<GetStudentTriageDataByDateQuery, List<StudentTriageDataShared>>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataByDateQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<StudentTriageDataShared>> Handle(GetStudentTriageDataByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<List<StudentTriageDataShared>>(new GetStudentTriageDataByDateRequest(request.ToDate, request.FromDate));

            result.EnsureSuccessStatusCode();

            var listResult = new List<StudentTriageDataShared>();

            foreach(var student in result.Body)
            {
                var test = new StudentSurveyDtoShared
                {
                    Id = student.StudentSurvey.Id,
                    StudentId = student.StudentSurvey.StudentId,
                    SurveyId = student.StudentSurvey.SurveyId,
                    LastUpdated = student.StudentSurvey.LastUpdated,
                    DateCompleted = student.StudentSurvey.DateCompleted,
                    DateEmailSent = student.StudentSurvey.DateEmailSent,
                    DateAdded = student.StudentSurvey.DateAdded,
                };

                listResult.Add(new StudentTriageDataShared
                {
                    Id = student.Id,
                    LepDateSent = student.LepDateSent,
                    LepsId = student.LepsId,
                    LepCode = student.LepCode,
                    LogId = student.LogId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    SchoolName = student.SchoolName,
                    URN = student.URN,
                    Email = student.Email,
                    Telephone = student.Telephone,
                    Postcode = student.Postcode,
                    DataSource = student.DataSource,
                    Industry = student.Industry,
                    DateInterest = student.DateInterest,
                    StudentSurvey = test,
                    SurveyQuestions = (ICollection<SurveyQuestionsDtoShared>)student.SurveyQuestions,
                });
            }

            return listResult;       
        }
    }
}