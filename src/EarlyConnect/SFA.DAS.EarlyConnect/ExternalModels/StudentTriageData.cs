using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.ExternalModels
{
    public  class StudentTriageData : StudentTriageDataBase
    {        
        public int? LepsId { get; set; }
        public int? LogId { get; set; }       
        public ICollection<SurveyQuestionsDto> SurveyResponses { get; set; }
        public StudentSurveyDto StudentSurvey { get; set; }
    }
}
