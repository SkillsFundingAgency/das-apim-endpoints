using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class StudentFeedbackMapper
    {
        public static StudentFeedbackList MapFromCreateStudentFeedbackRequest(this CreateStudentFeedbackPostRequest request, int LogId)
        {
            var studentFeedbackList = new List<StudentFeedback>();

            foreach (StudentFeedbackRequestModel dto in request.ListOfStudentFeedback)
            {
                var studentFeedback = new StudentFeedback
                {
                    SurveyId = dto.SurveyId,
                    StatusUpdate = dto.StatusUpdate,
                    Notes = dto.Notes,
                    UpdatedBy = dto.UpdatedBy,
                    LogId = LogId
                };

                studentFeedbackList.Add(studentFeedback);
            }

            return new StudentFeedbackList { ListOfStudentFeedback = studentFeedbackList };
        }
    }
}
