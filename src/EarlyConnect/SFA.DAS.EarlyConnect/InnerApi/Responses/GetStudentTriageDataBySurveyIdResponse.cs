using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetStudentTriageDataBySurveyIdResponse
    {
        public int Id { get; set; }
        public int? LepsId { get; set; }
        public int? LogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string DataSource { get; set; }
        public string Industry { get; set; }
        public DateTime? DateInterest { get; set; }
        public ICollection<SurveyQuestionsDto> SurveyQuestions { get; set; }
        public StudentSurveyDto StudentSurvey { get; set; }
    }
}

