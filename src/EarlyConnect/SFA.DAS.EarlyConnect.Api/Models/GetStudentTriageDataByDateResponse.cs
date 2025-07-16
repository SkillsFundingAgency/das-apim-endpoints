using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetStudentTriageDataByDateResponse
    {
        public int Id { get; set; }
        public DateTime? LepDateSent { get; set; }
        public int? LepsId { get; set; }
        public string LepCode { get; set; }
        public int? LogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string SchoolName { get; set; }
        public string URN { get; set; } = string.Empty;
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string DataSource { get; set; }
        public string Industry { get; set; }
        public DateTime? DateInterest { get; set; }
        public ICollection<SurveyQuestions> SurveyQuestions { get; set; }
        public StudentSurvey StudentSurvey { get; set; }
        public static implicit operator GetStudentTriageDataByDateResponse(GetStudentTriageDataByDateResult source)
        {
            return new GetStudentTriageDataByDateResponse
            {
                Id = source.Id,
                LepDateSent = source.LepDateSent,
                LepsId = source.LepsId,
                LepCode = source.LepCode,
                LogId = source.LogId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth,
                SchoolName= source.SchoolName,
                URN=source.URN,
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
}

