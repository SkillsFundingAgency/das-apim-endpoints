using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class CreateOtherStudentTriageDataPostResponse
    {
        public string StudentSurveyId { get; set; }
        public string AuthCode { get; set; }
        public DateTime Expiry { get; set; }
        public static implicit operator CreateOtherStudentTriageDataPostResponse(CreateOtherStudentTriageDataCommandResult source)
        {
            return new CreateOtherStudentTriageDataPostResponse
            {
                StudentSurveyId = source.StudentSurveyId,
                AuthCode = source.AuthCode,
                Expiry = source.ExpiryDate
            };
        }
    }
}
