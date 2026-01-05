using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Models
{
    public  class StudentTriageData : StudentTriageDataBase
    {        
        public int? LepsId { get; set; }
        public int? LogId { get; set; }        
        public StudentSurveyDto StudentSurvey { get; set; }
    }
}
