using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class StudentSurveyDtoShared
    {
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateEmailSent { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<ResponseAnswersDto> ResponseAnswers { get; set; }
    }

    public class ResponseAnswersDto
    {
        public int? Id { get; set; }
        public Guid StudentSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
