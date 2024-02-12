using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetStudentTriageDataBySurveyIdResponse
    {
        public int Id { get; set; }
        public int? LepsId { get; set; }
        public int? LogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string SchoolName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string DataSource { get; set; }
        public string Industry { get; set; }
        public DateTime? DateInterest { get; set; }
        public ICollection<SurveyQuestions> SurveyQuestions { get; set; }
        public StudentSurvey StudentSurvey { get; set; }
        public static implicit operator GetStudentTriageDataBySurveyIdResponse(GetStudentTriageDataBySurveyIdResult source)
        {
            return new GetStudentTriageDataBySurveyIdResponse
            {
                Id = source.Id,
                LepsId = source.LepsId,
                LogId = source.LogId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth,
                SchoolName= source.SchoolName,
                Email = source.Email,
                Telephone = source.Telephone,
                Postcode = source.Postcode,
                DataSource = source.DataSource,
                Industry = source.Industry,
                DateInterest = source.DateInterest,
                StudentSurvey = source.StudentSurvey,
                SurveyQuestions = source.SurveyQuestions.Select(c => (SurveyQuestions)c).ToList()
            };
        }
    }
    public class SurveyQuestions
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }
        public string ShortDescription { get; set; }
        public string SummaryLabel { get; set; }
        public string ValidationMessage { get; set; }
        public int? DefaultToggleAnswerId { get; set; }
        public int SortOrder { get; set; }
        public ICollection<Answers> Answers { get; set; }
        public static implicit operator SurveyQuestions(SurveyQuestionsDto source)
        {
            return new SurveyQuestions
            {
                Id = source.Id,
                SurveyId = source.SurveyId,
                QuestionTypeId = source.QuestionTypeId,
                QuestionText = source.QuestionText,
                ShortDescription = source.ShortDescription,
                SummaryLabel = source.SummaryLabel,
                ValidationMessage = source.ValidationMessage,
                DefaultToggleAnswerId = source.DefaultToggleAnswerId,
                SortOrder = source.SortOrder,
                Answers = source.Answers.Select(c => (Answers)c).ToList()
            };
        }
    }
    public class Answers
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public string ShortDescription { get; set; }
        public int GroupNumber { get; set; }
        public string GroupLabel { get; set; }
        public int SortOrder { get; set; }
        public static implicit operator Answers(AnswersDto source)
        {
            return new Answers
            {
                Id = source.Id,
                QuestionId = source.QuestionId,
                AnswerText = source.AnswerText,
                ShortDescription = source.ShortDescription,
                GroupLabel = source.GroupLabel,
                GroupNumber = source.GroupNumber,
                SortOrder = source.SortOrder,
            };
        }
    }
    public class StudentSurvey
    {
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateEmailSent { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<ResponseAnswers> ResponseAnswers { get; set; }
        public static implicit operator StudentSurvey(StudentSurveyDto source)
        {
            return new StudentSurvey
            {
                Id = source.Id,
                StudentId = source.StudentId,
                SurveyId = source.SurveyId,
                LastUpdated = source.LastUpdated,
                DateCompleted = source.DateCompleted,
                DateEmailSent = source.DateEmailSent,
                DateAdded = source.DateAdded,
                ResponseAnswers = source.ResponseAnswers.Select(c => (ResponseAnswers)c).ToList()
            };
        }
    }
    public class ResponseAnswers
    {
        public int Id { get; set; }
        public Guid StudentSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
        public static implicit operator ResponseAnswers(ResponseAnswersDto source)
        {
            return new ResponseAnswers
            {
                Id = source.Id.Value,
                StudentSurveyId = source.StudentSurveyId,
                QuestionId = source.QuestionId,
                AnswerId = source.AnswerId,
                Response = source.Response,
                DateAdded = source.DateAdded
            };
        }
    }
}

