using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class ManageStudentTriageDataMapper
    {
        public static StudentTriageData MapFromManageStudentTriageDataRequest(this ManageStudentTriageDataPostRequest request)
        {

            StudentTriageData manageStudentTriageData = new StudentTriageData();


            manageStudentTriageData.Id = request.Id;
            manageStudentTriageData.LepsId = request.LepsId;
            manageStudentTriageData.LogId = request.LogId;
            manageStudentTriageData.FirstName = request.FirstName;
            manageStudentTriageData.LastName = request.LastName;
            manageStudentTriageData.DateOfBirth = request.DateOfBirth;
            manageStudentTriageData.Telephone = request.Telephone;
            manageStudentTriageData.Email = request.Email;
            manageStudentTriageData.Postcode = request.Postcode;
            manageStudentTriageData.DataSource = request.DataSource;
            manageStudentTriageData.Industry = request.Industry;
            manageStudentTriageData.DateInterest = request.DateInterest;

            manageStudentTriageData.StudentSurvey = new StudentSurveyDto();

            manageStudentTriageData.StudentSurvey.Id = request.StudentSurvey.Id;
            manageStudentTriageData.StudentSurvey.StudentId = request.StudentSurvey.StudentId;
            manageStudentTriageData.StudentSurvey.SurveyId = request.StudentSurvey.SurveyId;
            manageStudentTriageData.StudentSurvey.LastUpdated = request.StudentSurvey.LastUpdated;
            manageStudentTriageData.StudentSurvey.DateCompleted = request.StudentSurvey.DateCompleted;
            manageStudentTriageData.StudentSurvey.DateEmailSent = request.StudentSurvey.DateEmailSent;
            manageStudentTriageData.StudentSurvey.DateAdded = request.StudentSurvey.DateAdded;
            manageStudentTriageData.StudentSurvey.ResponseAnswers = new List<ResponseAnswersDto>();

            foreach (AnswersRequest dto in request.StudentSurvey.ResponseAnswers)
            {
                var answers = new ResponseAnswersDto()
                {
                    Id = dto.Id,
                    StudentSurveyId = dto.StudentSurveyId,
                    QuestionId = dto.QuestionId,
                    AnswerId = dto.AnswerId,
                    Response = dto.Response,
                    DateAdded = dto.DateAdded,

                };

                manageStudentTriageData.StudentSurvey.ResponseAnswers.Add(answers);
            }

            return  manageStudentTriageData;
        }
    }
}
